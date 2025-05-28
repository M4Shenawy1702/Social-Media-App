import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { Component, NgZone, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { PostsService } from '../Services/posts.service';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { CommentsService } from '../Services/comments.service';


declare const bootstrap: any;

@Component({
  selector: 'app-Posts',
  standalone: true,
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss'],
  imports: [CommonModule, NgbModule, RouterModule, FormsModule],
})
export class PostsComponent implements OnInit {
  baseUrl = 'http://localhost:5043/';
  isLoading = false;
  errorMessage = '';
  postContent: string = '';
  commentContents: { [postId: number]: string } = {};
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
    private authService: AuthServiceService,
    private commentservice: CommentsService,
  ) { }

  ngOnInit(): void {
    this.CurrentUserId = this.authService.getCurrentUserId();
    this.loadPosts();
  }

  loadPosts(): void {
    this.isLoading = true;
    this.postsService.getAllPosts(this.queryParams).subscribe({
      next: (response) => {
        this.posts = response;
          
        this.queryParams.pageIndex = response.pageIndex;
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

  onLike(post: Post): void {
    const wasLiked = post.isLiked;

    this.postsService.onLike(post.id).subscribe({
      next: () => {
        const updatedPost = {
          ...post,
          isLiked: !wasLiked,
          likes: post.likes + (wasLiked ? -1 : 1)
        };

        this.posts.data = this.posts.data.map(p =>
          p.id === post.id ? updatedPost : p
        );

      },
      error: (error) => {
        console.error('Error liking post:', error);
      }
    });
  }

  addComment(postId: number): void {
    if (!this.commentContents[postId] || !this.commentContents[postId].trim() || !this.CurrentUserId) {
      this.errorMessage = 'Post content is required.';
      return;
    }

    const formData = new FormData();
    formData.append('Content', this.commentContents[postId]);
    formData.append('AuthorId', this.CurrentUserId);
    formData.append('PostId', postId.toString());

    this.commentservice.addComment(formData).subscribe({
      next: (newComment) => {
        const post = this.posts.data.find(p => p.id === postId);
        if (post) {
          console.log(newComment);
          post.comments = [...post.comments, newComment];
        }
        this.commentContents[postId] = '';
      },
      error: (error) => {
        console.error('Error adding comment:', error);
      }
    });
  }
  onPageChange(page: number): void {
    this.queryParams.pageIndex = page;
    this.loadPosts();
  }
  isVideo(url: string): boolean {
    return /\.(mp4|webm|ogg)$/i.test(url);
  }
  isImage(url: string): boolean {
    return /\.(jpeg|jpg|png|gif|bmp|svg)$/i.test(url);
  }
}
