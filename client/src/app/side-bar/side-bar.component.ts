import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import {CurrentUser} from '../shared/Contracts/CurrentUser'
 
@Component({
  selector: 'app-side-bar',
  imports: [RouterLink],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.scss'
})
export class SideBarComponent implements OnInit {

  user : CurrentUser  = JSON.parse(localStorage.getItem('user') || '{}');
  currentUserId : string = '';
  
  constructor(private authService:AuthServiceService) {
  }
  ngOnInit(): void {
    this.currentUserId = this.getUserId();
  }

  getUserId(): string {
    return this.authService.getCurrentUserId();
  }

}
