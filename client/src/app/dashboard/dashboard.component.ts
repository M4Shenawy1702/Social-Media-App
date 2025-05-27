import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { UserProfile } from '../shared/Contracts/UserProfile';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NgbModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {

  title = 'The ChattingApp';
  users: PagenatedResult<UserProfile> = {
    pageIndex: 1,
    pageSize: 10,
    count: 0,
    data: []
  };

  isLoading = false;
  errorMessage = '';

  baseUrl = 'http://localhost:5043/';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.isLoading = true;
    const token = localStorage.getItem('jwtToken');

    if (!token) {
      this.isLoading = false;
      this.errorMessage = 'No token found. Please log in.';
      return;
    }

    this.http.get<PagenatedResult<UserProfile>>(`${this.baseUrl}api/users/`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    }).subscribe({
      next: (response) => {
        console.log('Data received:', response);

        const currentUser = JSON.parse(localStorage.getItem('user') || '{}');
        const currentUserId = currentUser?.userId;

        this.users.data = response.data.filter(user => user.id !== currentUserId);
        this.users.count = response.count;
        this.users.pageIndex = response.pageIndex;
        this.users.pageSize = response.pageSize;

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
