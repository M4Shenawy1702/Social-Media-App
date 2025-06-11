import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';

@Component({
  selector: 'app-connections',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './connections.component.html',
  styleUrl: './connections.component.scss'
})
export class ConnectionsComponent implements OnInit {
  currentUserId: string = '';

  constructor(private authService: AuthServiceService) {

  }
  ngOnInit(): void {
    this.currentUserId = this.getUserId();
  }


  getUserId(): string {
    return this.authService.getCurrentUserId();
  }
}
