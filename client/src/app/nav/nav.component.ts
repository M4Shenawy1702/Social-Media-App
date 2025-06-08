import { Component, OnInit } from '@angular/core';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import {CurrentUser} from '../shared/Contracts/CurrentUser'
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: CurrentUser = {} as CurrentUser;
  baseUrl: string = 'http://localhost:5043/';
  showLoggedInNav: boolean = false;
  currentUserId: string = '';

  constructor(public authService: AuthServiceService, private router: Router) {
    this.loadUser();
    this.showLoggedInNav = this.authService.isLoggedIn();
  }
  ngOnInit(): void {
    this.currentUserId = this.getUserId()
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
}

