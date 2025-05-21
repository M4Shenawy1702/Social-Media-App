import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';

@Component({
  selector: 'app-friend-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss']
})
export class FriendsListComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  friendRequests: any[] = [];

  constructor(private _authService: AuthServiceService, private http: HttpClient) {}

  ngOnInit(): void {
    this.getallfriends();
  }
getallfriends(): void {
  this.isLoading = true;
  const currentUserId: string = this._authService.getCurrentUserId();
  const token = localStorage.getItem('jwtToken');

  const headers = token
    ? { Authorization: `Bearer ${token}` }
    : {};

  this.http.get<any[]>(`http://localhost:5043/api/UserRelationships/get-friends/${currentUserId}`, { headers }).subscribe({
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
}
