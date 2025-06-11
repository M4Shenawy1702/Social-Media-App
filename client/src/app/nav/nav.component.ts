import { UserService } from './../Services/user.service';
import { Component, NgModule, OnInit } from '@angular/core';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { CurrentUser } from '../shared/Contracts/CurrentUser'
import { debounceTime, Subject } from 'rxjs';
import { FormsModule, NgModel } from '@angular/forms';
import { UserQueryParameters } from '../shared/Contracts/UserQueryParameters';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: CurrentUser = {} as CurrentUser;
  baseUrl: string = 'http://localhost:5043/';
  showLoggedInNav: boolean = false;
  currentUserId: string = '';
  searchTerm = '';
  searchResults: any[] = [];
  searchChanged = new Subject<string>();

  constructor(public authService: AuthServiceService, private router: Router, private userService: UserService) {
    this.loadUser();
    this.showLoggedInNav = this.authService.isLoggedIn();
  }
  ngOnInit(): void {
    this.currentUserId = this.getUserId()
    this.searchChanged.pipe(debounceTime(300)).subscribe(() => {
      this.searchUsers();
    });
  }

  loadUser() {
    const userData = localStorage.getItem('user');
    this.user = userData ? JSON.parse(userData) : ({} as CurrentUser);
  }

  getUserId(): string {
    return this.authService.getCurrentUserId();
  }

  logout(): void {
    this.authService.logout();
    this.showLoggedInNav = false;
    this.user = {} as CurrentUser;

  }
  onSearchChange(): void {
    this.searchChanged.next(this.searchTerm);
  }

  searchUsers(): void {
    const trimmed = this.searchTerm.trim();

    if (!trimmed) {
      this.searchResults = [];
      return;
    }

    const params: UserQueryParameters = {
      pageIndex: 1,
      pageSize: 10,
      searchByName: trimmed
    };

    this.userService.getUsers(params).subscribe({
      next: (results) => {
        console.log('Search results:', results);
        this.searchResults = results.data;
      },
      error: (err) => {
        console.error('Error searching users:', err);
        this.searchResults = [];
      }
    });
  }



  goToUserProfile(userId: string): void {
    this.searchResults = [];
    this.searchTerm = '';
    this.router.navigate(['/main-layout/profile', userId]);
  }
}

