import { UserService } from './../Services/user.service';
import { FriendService } from './../Services/friend.service';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { UserProfile } from '../shared/Contracts/UserProfile';
import { FriendStatus } from "../shared/Contracts/FriendStatus";
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { UserQueryParameters } from '../shared/Contracts/UserQueryParameters';
import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { environment } from '../../environments/environment'; 
import e from 'express';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  users: PagenatedResult<UserProfile> = {
    pageIndex: 1,
    pageCount: 10,
    count: 0,
    data: []
  };
  pages: number[] = [];
  isLoading = false;
  noMoreUsers = false;
  errorMessage = '';
  baseUrl = environment.baseUrl;

  constructor(
    private http: HttpClient,
    private friendService: FriendService,
    private AuthServiceService: AuthServiceService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.isLoading = true;

    const queryParams: UserQueryParameters = {
      pageIndex: this.users.pageIndex,
      pageSize: this.users.pageCount,
      searchByName: undefined,
    };

    console.log('Sending queryParams:', queryParams);

    this.userService.getUsers(queryParams).subscribe({
      next: (response) => {
        console.log('Data received:', response);

        const newUsers = response.data.filter(
          user => !this.users.data.some(existing => existing.id === user.id)
        );

        this.users.data = [...this.users.data, ...newUsers];
        this.users.count = response.count;
        this.users.pageCount = response.pageCount;

        if (this.users.pageIndex >= this.users.pageCount) {
          this.noMoreUsers = true;
        }

        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load the data';
        console.error(err);
      }
    });
  }

  LoadMore(): void {
    if (this.users.pageIndex >= this.users.pageCount) {
      console.log('No more pages to load');
      this.noMoreUsers = true;
      return;
    }

    this.users.pageIndex++;
    this.getUsers();
  }

  trackByUserId(index: number, user: UserProfile): string {
    return user.id;
  }
}
