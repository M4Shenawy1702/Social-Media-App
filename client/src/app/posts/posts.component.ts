import { PostComment } from './../shared/Contracts/PostComment';
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Post } from '../shared/Contracts/Post';
import { PostsService } from '../Services/posts.service';
import { CommentsService } from '../Services/comments.service';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import Swal from 'sweetalert2';
import { Router, RouterLink } from '@angular/router';
declare var bootstrap: any;


@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss']
})
export class PostsComponent {

  @Input() posts: Post[] = [];
  selectedPost: Post = null
  selectedComment: PostComment = null
  baseUrl = 'http://localhost:5043/';

  commentContents: { [postId: number]: string } = {};
  CurrentUserId: string | null;

  constructor(
    private postService: PostsService,
    private commentService: CommentsService,
    private authService: AuthServiceService,
    private router: Router
  ) {
    this.CurrentUserId = this.authService.getCurrentUserId();
  }

  onLike(post: Post): void {
    const wasLiked = post.isLiked;

    this.postService.onLike(post.id).subscribe({
      next: () => {
        post.isLiked = !wasLiked;
        post.likes += wasLiked ? -1 : 1;
      },
      error: (error) => console.error('Error liking post:', error)
    });
  }

  addComment(postId: number): void {
    const content = this.commentContents[postId]?.trim();
    if (!content || !this.CurrentUserId) return;

    const formData = new FormData();
    formData.append('Content', content);
    formData.append('AuthorId', this.CurrentUserId);
    formData.append('PostId', postId.toString());

    this.commentService.addComment(formData).subscribe({
      next: (newComment) => {
        const post = this.posts.find(p => p.id === postId);
        if (post) post.comments.push(newComment);
        this.commentContents[postId] = '';
        Swal.fire('Success!', 'Comment added successfully.', 'success');
      },
      error: (error) => {
        console.error('Error adding comment:', error);
        Swal.fire('Error', 'Failed to add comment.', 'error');
      }
    });
  }

  onDelete(postId: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You will not be able to recover this post!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          title: 'Deleting...',
          allowOutsideClick: false,
          didOpen: () => {
            Swal.showLoading();
          }
        });

        this.postService.deletePost(postId).subscribe({
          next: () => {
            this.posts = this.posts.filter(post => post.id !== postId);
            Swal.fire('Deleted!', 'The post has been deleted.', 'success');
          },
          error: (error) => {
            console.error('Error deleting post:', error);
            Swal.fire('Error', 'Something went wrong. Please try again.', 'error');
          }
        });
      }
    });
  }
  openEditPostModal(post: Post) {
    this.selectedPost = { ...post };
    const modalElement = document.getElementById('editPostModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }
  onEditPost(post: Post) {
    const formData = new FormData();
    formData.append('Content', post.content);
    this.postService.updatePost(post.id, formData).subscribe({
      next: () => {
        const index = this.posts.findIndex(p => p.id === post.id);
        if (index !== -1) this.posts[index] = post;

        const modalElement = document.getElementById('editPostModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal?.hide();
        }

        Swal.fire('Success!', 'Post updated successfully.', 'success');
      },
      error: (error) => {
        console.error('Error updating post:', error);
        Swal.fire('Error', 'Failed to update post.', 'error');
      }
    });
  }

  onDeleteComment(commentId: number, postId: number) {
    Swal.fire({
      title: 'Are you sure?',
      text: 'This comment will be permanently deleted.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, cancel'
    }).then((result) => {
      if (result.isConfirmed) {
        this.commentService.deleteComment(commentId).subscribe({
          next: () => {
            const post = this.posts.find(p => p.id === postId);
            if (post) {
              post.comments = post.comments.filter(comment => comment.id !== commentId);
            }
            Swal.fire('Deleted!', 'Comment has been deleted.', 'success');
          },
          error: (error) => {
            console.error('Error deleting comment:', error);
            Swal.fire('Error', 'Failed to delete comment.', 'error');
          }
        });
      }
    });
  }

  onEditComment(comment: PostComment) {
    const formData = new FormData();
    formData.append('Content', comment.content);
    formData.append('AuthorId', comment.authorId.toString());
    formData.append('PostId', comment.postId.toString());

    this.commentService.updateComment(comment.id, formData).subscribe({
      next: () => {
        const post = this.posts.find(p => p.id === comment.postId);
        if (post) {
          post.comments = post.comments.map(c => c.id === comment.id ? comment : c);
        }

        const modalElement = document.getElementById('editCommentModal');
        if (modalElement) {
          const modal = bootstrap.Modal.getInstance(modalElement);
          modal?.hide();
        }

        Swal.fire('Updated!', 'Comment updated successfully.', 'success');
      },
      error: (error) => {
        console.error('Error updating comment:', error);
        Swal.fire('Error', 'Failed to update comment.', 'error');
      }
    });
  }

  isVideo(url: string): boolean {
    return /\.(mp4|webm|ogg)$/i.test(url);
  }

  isImage(url: string): boolean {
    return /\.(jpeg|jpg|png|gif|bmp|svg)$/i.test(url);
  }
}
