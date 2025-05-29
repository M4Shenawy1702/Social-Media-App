import { CurrentUser } from './../shared/Contracts/CurrentUser';
import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FriendService } from '../Services/friend.service';
import { PostsService } from '../Services/posts.service'; 
import { UserProfile } from '../shared/Contracts/UserProfile'; 
import { Post } from '../shared/Contracts/Post';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { PostsComponent } from "../posts/posts.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterLink, PostsComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  baseUrl = 'http://localhost:5043/';
  userId: string | null = null;
  CurrentUserId: string | null = null;
  userProfile: UserProfile | null = null;
  isLoading = false;
  errorMessage = '';

  posts: PagenatedResult<Post> = {
    pageIndex: 1,
    pageSize: 10,
    count: 0,
    data: []
  };

  constructor(
    private http: HttpClient, 
    private route: ActivatedRoute,
    private friendService: FriendService,
    private postsService: PostsService ,
    private AuthServiceService: AuthServiceService
  ) {}

 ngOnInit(): void {
  this.route.paramMap.subscribe(params => {
    this.userId = params.get('id');
    if (this.userId) {
      this.getProfile(this.userId);
      this.getCurrentUserPost(this.userId); 
      this.CurrentUserId = this.AuthServiceService.getCurrentUserId();
    }
  });
}

  getProfile(userId: string) {
    const token = localStorage.getItem('jwtToken');
    const headers = { 'Authorization': `Bearer ${token}` };

    this.http.get<UserProfile>(`${this.baseUrl}api/users/${userId}`, { headers })
      .subscribe({
        next: (res) => this.userProfile = res,
        error: (err) => console.error('Failed to load user profile:', err)
      });
  }

  getCurrentUser(): CurrentUser | null {
    try {
      const userString = localStorage.getItem('user');
      if (!userString) return null;
      return JSON.parse(userString) as CurrentUser;
    } catch (error) {
      console.error('Error parsing user data:', error);
      return null;
    }
  }

  getCurrentUserPost(userId: string) {
    this.isLoading = true;

    const params: PostQueryParameters = {
      pageIndex: this.posts.pageIndex,
      pageSize: this.posts.pageSize,
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

  onAddFriend(receiverId: string) {
    const currentUser = this.getCurrentUser();
    if (!currentUser || this.isLoading) return;

    this.isLoading = true;
    this.friendService.addFriend(currentUser.userId, receiverId).subscribe({
      next: (res) => {
        console.log('Friend added!', res);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error adding friend:', err);
        this.isLoading = false;
      }
    });
  }
}
