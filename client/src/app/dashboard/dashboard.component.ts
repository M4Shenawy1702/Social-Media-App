import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
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
  users: UserProfile[] = [];
  isLoading = false;
  errorMessage = '';
  string: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers();
  }
  baseUrl = 'http://localhost:5043/'
  getUsers(): void {
    this.isLoading = true;
    const token = localStorage.getItem('jwtToken');
  
    if (token) {
      this.http.get('http://localhost:5043/api/users/', {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }).subscribe({
        next: (response: any[]) => {
          // console.log('Data received:', response);
  
          const currentUser = JSON.parse(localStorage.getItem('user') || '{}');
          const currentUserId = currentUser?.userId;
          this.users = response.filter(user => user.id !== currentUserId);

          this.isLoading = false;
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Failed to load the data';
          console.log(err);
        }
      });
    } else {
      this.isLoading = false;
      this.errorMessage = 'No token found. Please log in.';
    }
  }
  
}
