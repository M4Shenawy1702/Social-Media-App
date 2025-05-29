import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Post } from '../shared/Contracts/Post';
import { PostsService } from '../Services/posts.service';
import { CommentsService } from '../Services/comments.service';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import  Swal from 'sweetalert2';
import { Router } from '@angular/router';



@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.scss']
})
export class PostsComponent {

  @Input() posts: Post[] = [];
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
      },
      error: (error) => console.error('Error adding comment:', error)
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
onPageChange(newPage: number) {
  // كود تغيير الصفحة أو إرسال الحدث للأب
}

isVideo(url: string): boolean {
  return /\.(mp4|webm|ogg)$/i.test(url);
}

isImage(url: string): boolean {
  return /\.(jpeg|jpg|png|gif|bmp|svg)$/i.test(url);
}
}
