import { CurrentUser } from './../shared/Contracts/CurrentUser';
import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FriendService } from '../Services/friend.service';
import { PostsService } from '../Services/posts.service'; 
import { UserProfile } from '../shared/Contracts/UserProfile'; 
import { Post } from '../shared/Contracts/Post';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { PostsComponent } from "../posts/posts.component";
import { FriendStatus } from "../shared/Contracts/FriendStatus";
import { environment } from '../../environments/environment'; 

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterLink, PostsComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  FriendStatus = FriendStatus; 
  baseUrl = environment.baseUrl;
  userId: string | null = null;
  CurrentUserId: string | null = null;
  userProfile: UserProfile | null = null;
  activeTab: 'posts' | 'likes' = 'posts';
  isLoading = false;
  errorMessage = '';
  friendStatus: FriendStatus = FriendStatus.None;  

  posts: PagenatedResult<Post> = {
    pageIndex: 1,
    pageCount: 10,
    count: 0,
    data: []
  };
  likedPosts: Post[] = [];

  constructor(
    private http: HttpClient, 
    private route: ActivatedRoute,
    private friendService: FriendService,
    private postsService: PostsService,
    private AuthServiceService: AuthServiceService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = params.get('id');
      if (this.userId) {
        this.CurrentUserId = this.AuthServiceService.getCurrentUserId();
        this.getProfile(this.userId);
        this.getCurrentUserPost(this.userId); 
        if (this.userId === this.CurrentUserId) {
          this.getAllLikedPosts();
        }
        this.getFriendStatus(this.userId);
      }
    });
  }

  getProfile(userId: string) {
    const token = localStorage.getItem('jwtToken');
    const headers = { 'Authorization': `Bearer ${token}` };

    this.http.get<UserProfile>(`${this.baseUrl}/api/users/${userId}`, { headers })
      .subscribe({
        next: (res) => this.userProfile = res,
        error: (err) => {
          console.error('Failed to load user profile:', err);
          this.errorMessage = 'Failed to load user profile.';
        }
      });
  }

  getCurrentUserPost(userId: string) {
    this.isLoading = true;

    const params: PostQueryParameters = {
      pageIndex: this.posts.pageIndex,
      pageSize: this.posts.pageCount,
      userId: userId
    };

    this.postsService.getAllPosts(params).subscribe({
      next: (response) => {
        this.posts = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load posts.';
        console.error(err);
      }
    });
  }

  getAllLikedPosts() {
    this.isLoading = true;

    this.postsService.getAllLikedPosts().subscribe({
      next: (response) => {
        this.likedPosts = response;
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load liked posts.';
        console.error(err);
      }
    });
  }

  onAddFriend(receiverId: string | null) {
    const currentUser = this.AuthServiceService.getCurrentUserId();
    if (!currentUser || this.isLoading || !receiverId) return;

    this.isLoading = true;
    this.friendService.addFriend(currentUser, receiverId).subscribe({
      next: (res) => {
        console.log('Friend added!', res);
        this.isLoading = false;
        this.friendStatus = FriendStatus.Pending;  
      },
      error: (err) => {
        console.error('Error adding friend:', err);
        this.isLoading = false;
      }
    });
  }

  getFriendStatus(friendId: string) {
    this.isLoading = true;
    this.friendService.getFriendStatus(friendId).subscribe({
      next: (res) => {
        console.log('Friend Status:', res);
        this.friendStatus = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching friend status:', err);
        this.isLoading = false;
      }
    });
  }
}
