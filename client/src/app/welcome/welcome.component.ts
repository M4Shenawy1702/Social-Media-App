import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PostsService } from '../Services/posts.service';
import { PostsComponent } from "../posts/posts.component";
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { CurrentUser } from '../shared/Contracts/CurrentUser';

declare const bootstrap: any;

@Component({
  selector: 'app-welcome',
  standalone: true,
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  imports: [CommonModule, NgbModule, RouterModule, FormsModule, PostsComponent],
})
export class WelcomeComponent implements OnInit {
  baseUrl = 'http://localhost:5043/';
  isLoading = false;
  errorMessage = '';
  postContent: string = '';
  selectedFiles: File[] = [];
  CurrentUserId: string | null = null;

  posts: PagenatedResult<Post> = {
    pageIndex: 1,
    pageSize: 10,
    count: 0,
    data: []
  };
  user: CurrentUser = {} as CurrentUser;
  constructor(
    private postsService: PostsService,
    private authService: AuthServiceService
  ) { }

  ngOnInit(): void {
    this.CurrentUserId = this.authService.getCurrentUserId();
    this.loadPosts();
    this.loadUser()
  }
  loadUser() {
    const userData = localStorage.getItem('user');
    this.user = userData ? JSON.parse(userData) : ({} as CurrentUser);
  }
  createPost(): void {
    if (!this.postContent.trim() || !this.CurrentUserId) {
      this.errorMessage = 'Post content is required.';
      return;
    }

    const formData = new FormData();
    formData.append('Content', this.postContent);
    formData.append('AuthorId', this.CurrentUserId);

    this.selectedFiles.forEach(file => {
      formData.append('Media', file);
    });

    this.postsService.createPost(formData).subscribe({
      next: () => {
        this.postContent = '';
        this.selectedFiles = [];
        const modalEl = document.getElementById('createPostModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl);
          modal?.hide();
        }
        this.loadPosts();
      },
      error: (err) => {
        this.errorMessage = err.message || 'Failed to create post.';
        console.error(err);
      }
    });
  }

  loadPosts() {
    this.isLoading = true;
    this.errorMessage = null;

    const params: PostQueryParameters = {
      pageIndex: this.posts.pageIndex,
      pageSize: this.posts.pageSize,
      // يمكن تضيف فلاتر إضافية هنا
    };

    this.postsService.getAllPosts(params).subscribe({
      next: (data) => {
        this.posts = data;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'حدث خطأ في تحميل البوستات.';
        this.isLoading = false;
      }
    });
  }

  handleFileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedFiles = Array.from(input.files);
    }
  }
}
