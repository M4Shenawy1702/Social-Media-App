import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';

@Component({
  selector: 'app-friend-request-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './friend-request-list.component.html',
  styleUrls: ['./friend-request-list.component.scss']
})
export class FriendRequestListComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  friendRequests: any[] = [];

  constructor(private _authService: AuthServiceService, private http: HttpClient) {}

  ngOnInit(): void {
    this.getallfriendrequests();
  }
getallfriendrequests(): void {
  this.isLoading = true;
  const userId: string = this._authService.getCurrentUserId();
  const currentUser = JSON.parse(localStorage.getItem('user') || '{}');
  const currentUserId = currentUser?.userId;
  const token = localStorage.getItem('jwtToken');

  const headers = token
    ? { Authorization: `Bearer ${token}` }
    : {};

  this.http.get<any[]>(`http://localhost:5043/api/UserRelationships/get-received-requests/${userId}`, { headers }).subscribe({
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
}
