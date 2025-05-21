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
export class SideBarComponent {

  user : CurrentUser  = JSON.parse(localStorage.getItem('user') || '{}');

  constructor(private authService:AuthServiceService) {
  }

  getUserId(): string {
    return this.authService.getCurrentUserId();
  }

}
