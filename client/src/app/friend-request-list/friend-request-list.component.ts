import { FriendListDetails } from './../shared/Contracts/FreindRequestDetails';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { FriendReceivedListDetails } from '../shared/Contracts/FriendReceivedRequestDetailsDto';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-friend-request-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './friend-request-list.component.html',
  styleUrls: ['./friend-request-list.component.scss']
})
export class FriendRequestListComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  friendRequests: FriendReceivedListDetails[] = [];
  baseUrl = 'http://localhost:5043/';


  constructor(private _authService: AuthServiceService, private http: HttpClient) { }

  ngOnInit(): void {
    this.getallfriendrequests();
  }
  getallfriendrequests(): void {
    this.isLoading = true;
    const currentUserId: string = this._authService.getCurrentUserId();
    const token = localStorage.getItem('jwtToken');

    const headers = token
      ? { Authorization: `Bearer ${token}` }
      : {};

    this.http.get<FriendReceivedListDetails[]>(`http://localhost:5043/api/UserRelationships/get-received-requests/`, { headers }).subscribe({
      next: (response) => {
        console.log('Data received:', response);
        this.friendRequests = response.filter(user => user.id !== currentUserId);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load the data';
        console.error(err);
      }
    });
  }


  respondToRequest(requestId: string, accept: boolean): void {
    this.isLoading = true;
    const token = localStorage.getItem('jwtToken');
    const endpoint = accept ? 'accept-request' : 'decline-request';

    if (token) {
      this.http.post(
        `http://localhost:5043/api/UserRelationships/${endpoint}/${requestId}`,
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      ).subscribe({
        next: () => {
          this.getallfriendrequests(); // Refresh list
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
