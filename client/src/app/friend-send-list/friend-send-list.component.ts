import { FriendService } from './../Services/friend.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient} from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-friend-request-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './friend-send-list.component.html',
  styleUrls: ['./friend-send-list.component.scss']
})
export class FriendSendListComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  friendSentRequests: any[] = [];

  constructor(private _authService: AuthServiceService, private http: HttpClient , private friendService : FriendService) {}

  ngOnInit(): void {
    this.getallsentrequests();
  }
getallsentrequests(): void {
  this.isLoading = true;
  const currentUserId: string = this._authService.getCurrentUserId();
  const token = localStorage.getItem('jwtToken');

  const headers = token
    ? { Authorization: `Bearer ${token}` }
    : {};

  this.http.get<any[]>(`http://localhost:5043/api/UserRelationships/get-sent-requests`, { headers }).subscribe({
    next: (response) => {
      console.log('Data received:', response);
      this.friendSentRequests = response.filter(user => user.id !== currentUserId);
      this.isLoading = false;
    },
    error: (err) => {
      this.isLoading = false;
      this.errorMessage = 'Failed to load the data';
      console.error(err);
    }
  });
}

  onCancelRequest(requestId: string, accept: boolean): void {
    this.isLoading = true;
    const token = localStorage.getItem('jwtToken');
    const endpoint = accept ? 'accept-request' : 'decline-request';

    if (token) {
      this.friendService.cancelRequest(requestId).subscribe({
        next: () => {
          this.getallsentrequests();
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = `Failed to ${accept ? 'accept' : 'reject'} request`;
          console.error(err);
        }
      });
    } else {
      this.isLoading = false;
      this.errorMessage = 'No token found. Please log in.';
    }
  }
    getTimeAgo(createdAt: string): string {
    const date = new Date(createdAt);
    const now = new Date();
    const diff = now.getTime() - date.getTime();

    const seconds = Math.floor(diff / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);

    if (days > 0) return `${days} day${days > 1 ? 's' : ''} ago`;
    if (hours > 0) return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    if (minutes > 0) return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    return 'Just now';
  }
}
