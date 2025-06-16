import { UserService } from './../Services/user.service';
import { FriendService } from './../Services/friend.service';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { UserProfile } from '../shared/Contracts/UserProfile';
import { FriendStatus } from "../shared/Contracts/FriendStatus";
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { UserQueryParameters } from '../shared/Contracts/UserQueryParameters';

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
  pages: number[] = [];
  isLoading = false;
  errorMessage = '';
  baseUrl = 'http://localhost:5043/';

  constructor(private http: HttpClient, private friendService: FriendService, private AuthServiceService: AuthServiceService, private userService: UserService) { }

  ngOnInit(): void {
    this.getUsers();
  }
  get totalPages(): number {
    return Math.ceil(this.users.count / this.users.pageSize);
  }
  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.users.pageIndex = page;
    this.getUsers();
  }

  generatePages(): void {
    const maxPagesToShow = 5;
    const currentPage = this.users.pageIndex;
    const totalPages = this.totalPages;

    let start = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
    let end = Math.min(totalPages, start + maxPagesToShow - 1);

    if (end - start < maxPagesToShow - 1) {
      start = Math.max(1, end - maxPagesToShow + 1);
    }

    this.pages = [];
    for (let i = start; i <= end; i++) {
      this.pages.push(i);
    }
  }
  getUsers(): void {
    this.isLoading = true;

    const queryParams: UserQueryParameters = {
      pageIndex: this.users.pageIndex,
      pageSize: this.users.pageSize,
      searchByName: undefined,
    };

    this.userService.getUsers(queryParams).subscribe({
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
