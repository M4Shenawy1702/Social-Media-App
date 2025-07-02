import { AuthServiceService } from './../Services/AuthService/auth-service.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendService } from '../Services/friend.service';
import { FriendListDetails } from '../shared/Contracts/FreindRequestDetails';
import { environment } from '../../environments/environment'; 

@Component({
  selector: 'app-chat-dash-board',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './chat-dash-board.component.html',
  styleUrls: ['./chat-dash-board.component.scss']
})
export class ChatDashBoardComponent implements OnInit {
  friends: FriendListDetails[] = [];
  baseUrl = environment.baseUrl;
  userId: string
  currentUserId: string

  constructor(
    private friendService: FriendService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthServiceService
  ) { }

  ngOnInit(): void {
      this.currentUserId = this.authService.getCurrentUserId();

      this.loadFriends(this.currentUserId);
  }

  loadFriends(userId: string): void {
    this.friendService.getFriendList(userId).subscribe({
      next: (res) => {
        this.friends = res;
        console.log(this.friends);
      },
      error: (err) => console.error('Failed to load friends:', err)
    });
  }
}
