import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendService } from '../Services/friend.service';
import { FriendListDetails } from '../shared/Contracts/FreindRequestDetails';

@Component({
  selector: 'app-chat-dash-board',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './chat-dash-board.component.html',
  styleUrls: ['./chat-dash-board.component.scss']
})
export class ChatDashBoardComponent implements OnInit {
  friends: FriendListDetails[] = [];
  baseUrl = 'http://localhost:5043/';
  userId: string

  constructor(
    private friendService: FriendService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
      this.loadFriends(this.userId);
    })
  }

  loadFriends(userId: string): void {
    this.friendService.getFriendList(userId).subscribe({
      next: (res) => this.friends = res,
      error: (err) => console.error('Failed to load friends:', err)
    });
  }
}
