import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FriendService } from '../Services/friend.service';
import { UserProfile } from '../shared/Contracts/UserProfile'; 
import { CurrentUser } from '../shared/Contracts/CurrentUser'; 


@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  baseUrl = 'http://localhost:5043/';

  userId: string | null = null;
  userProfile: UserProfile | null = null;
  isLoading = false;

  user : string  = localStorage.getItem('user');
  
  constructor(
    private http: HttpClient, 
    private route: ActivatedRoute,
    private friendService: FriendService
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id');

    if (this.userId) {
      this.getProfile(this.userId);
    }
  }

  getProfile(userId: string) {
    const token = localStorage.getItem('jwtToken');
    
    const headers = {
      'Authorization': `Bearer ${token}`
    };
    
    this.http.get<UserProfile>(`${this.baseUrl}api/users/${userId}`, { headers })
      .subscribe({
        next: (res) => {
          this.userProfile = res;
        },
        error: (err) => {
          console.error('Failed to load user profile:', err);
        }
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
  onAddFriend(receiverId: string) {
    const currentUser = this.getCurrentUser();
    if (this.isLoading) return;

    this.isLoading = true;
    this.friendService.addFriend(currentUser.userId,receiverId).subscribe({
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