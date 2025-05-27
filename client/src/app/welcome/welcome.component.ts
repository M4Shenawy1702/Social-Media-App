import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { PostsService } from '../Services/posts.service';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';

declare const bootstrap: any; // For Bootstrap modals

@Component({
  selector: 'app-welcome',
  standalone: true,
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  imports: [CommonModule, NgbModule, RouterModule, FormsModule],
})
export class WelcomeComponent implements OnInit {
  baseUrl = 'http://localhost:5043/';
  isLoading = false;
  errorMessage = '';
  postContent: string = '';
  selectedFiles: File[] = [];

  CurrentUserId: string | null = null;

  posts: PagenatedResult<Post> = {
    data: [],
    count: 0,
    pageIndex: 1,
    pageSize: 10
  };

  queryParams: PostQueryParameters = {
    pageIndex: 1,
    pageSize: 10,
    search: '',
    userId: ''
  };

  constructor(
    private postsService: PostsService,
    private authService: AuthServiceService
  ) {}

  ngOnInit(): void {
    this.CurrentUserId = this.authService.getCurrentUserId();
    this.queryParams.userId = this.CurrentUserId ?? '';
    this.loadPosts();
  }

  loadPosts(): void {
    this.isLoading = true;
    this.postsService.getAllPosts(this.queryParams).subscribe({
      next: (response) => {
        this.posts = response;
        this.queryParams.pageIndex = response.pageIndex; // sync page
        this.queryParams.pageSize = response.pageSize;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.message || 'Failed to load posts.';
        console.error(err);
      }
    });
  }

  createPost(): void {
  if (!this.postContent.trim() || !this.CurrentUserId) {
    this.errorMessage = 'Post content is required.';
    return;
  }

  const formData = new FormData();
  formData.append('Content', this.postContent);
  formData.append('AuthorId', this.CurrentUserId);

  // Append files simply with the same key "Media"
  this.selectedFiles.forEach(file => {
    formData.append('Media', file);
  });

  this.postsService.createPost(formData).subscribe({
    next: () => {
      this.postContent = '';
      this.selectedFiles = [];
      this.loadPosts();
      const modalEl = document.getElementById('createPostModal');
      if (modalEl) {
        const modal = bootstrap.Modal.getInstance(modalEl);
        modal?.hide();
      }
    },
    error: (err) => {
      this.errorMessage = err.message || 'Failed to create post.';
      console.error(err);
    }
  });
}


  handleFileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      this.selectedFiles = Array.from(input.files);
    }
  }

  onLike(post: Post): void {
    console.log('Liked post:', post.id);
    // Like logic goes here
  }

  onPageChange(page: number): void {
    this.queryParams.pageIndex = page;
    this.loadPosts();
  }
}
