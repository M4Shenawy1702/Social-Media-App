import { InfiniteScrollDirective } from 'ngx-infinite-scroll';
import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PostsService } from '../Services/posts.service';
import { PostsComponent } from "../posts/posts.component";
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { CurrentUser } from '../shared/Contracts/CurrentUser';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import Swal from 'sweetalert2';
import { environment } from '../../environments/environment'; 



declare const bootstrap: any;

interface MediaPreview {
  file: File;
  url: string;
  safeUrl: SafeUrl;
  type: string;
  name: string;
}

@Component({
  selector: 'app-welcome',
  standalone: true,
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss'],
  imports: [CommonModule, RouterModule, FormsModule, PostsComponent, InfiniteScrollDirective],
})
export class WelcomeComponent implements OnInit {
  baseUrl = environment.baseUrl;
  isLoading = false;
  errorMessage = '';
  postContent: string = '';
  selectedFiles: File[] = [];
  mediaPreviews: MediaPreview[] = [];
  CurrentUserId: string | null = null;

  posts: PagenatedResult<Post> = {
    pageIndex: 1,
    pageCount: 10,
    count: 0,
    data: []
  };

  user: CurrentUser = {} as CurrentUser;

  constructor(
    private postsService: PostsService,
    private authService: AuthServiceService,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.CurrentUserId = this.authService.getCurrentUserId();
    this.loadPosts();
    this.loadUser();
  }

  loadUser() {
    const userData = localStorage.getItem('user');
    this.user = userData ? JSON.parse(userData) : ({} as CurrentUser);
  }

  createPost(): void {
    if (!this.postContent.trim() || !this.CurrentUserId) {
      Swal.fire('Error', 'Please enter a post content.', 'error');
      return;
    }

    const formData = new FormData();
    formData.append('Content', this.postContent);

    this.selectedFiles.forEach(file => {
      formData.append('Media', file);
    });

    this.postsService.createPost(formData).subscribe({
      next: () => {
        this.postContent = '';
        this.selectedFiles = [];
        this.mediaPreviews = [];

        const modalEl = document.getElementById('createPostModal');
        if (modalEl) {
          const modal = bootstrap.Modal.getInstance(modalEl);
          modal?.hide();
        }

        this.posts.pageIndex = 1;
        this.loadPosts();

        Swal.fire('Success', 'Post created successfully.', 'success');
      },
      error: (err) => {
        Swal.fire('Error', 'Failed to create post.', err.message);
      }
    });
  }

  loadPosts() {
    this.isLoading = true;
    this.errorMessage = '';

    const params: PostQueryParameters = {
      pageIndex: this.posts.pageIndex,
      pageSize: this.posts.pageCount,
    };

    this.postsService.getAllPosts(params).subscribe({
      next: (data) => {
        if (this.posts.pageIndex === 1) {
          this.posts = data;
        } else {
          this.posts.data = [...this.posts.data, ...data.data];
          this.posts.count = data.count;
        }

        this.isLoading = false;
      },
      error: () => {
        Swal.fire('Error', 'Failed to load posts.', 'error');
        this.isLoading = false;
      }
    });
  }
  onScrollDown(): void {
    if (this.posts.data.length >= this.posts.count) return;

    this.posts.pageIndex++;
    this.loadPosts();
  }

  handleFileInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    this.mediaPreviews.forEach(p => URL.revokeObjectURL(p.url));
    this.mediaPreviews = [];
    this.selectedFiles = [];

    Array.from(input.files).forEach(file => {
      const objectUrl = URL.createObjectURL(file);

      this.selectedFiles.push(file);
      this.mediaPreviews.push({
        file,
        url: objectUrl,
        safeUrl: this.sanitizer.bypassSecurityTrustUrl(objectUrl),
        type: file.type,
        name: file.name
      });
    });
  }

  removePreview(preview: MediaPreview): void {
    URL.revokeObjectURL(preview.url);
    this.mediaPreviews = this.mediaPreviews.filter(p => p !== preview);
    this.selectedFiles = this.selectedFiles.filter(f => f !== preview.file);
  }
}