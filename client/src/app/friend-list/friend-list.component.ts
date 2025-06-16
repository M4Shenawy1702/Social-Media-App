import Swal from 'sweetalert2';
import { FriendService } from './../Services/friend.service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { FriendListDetails } from '../shared/Contracts/FreindRequestDetails';
import { RouterLink } from '@angular/router';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-friend-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.scss']
})
export class FriendsListComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  friendList: FriendListDetails[] = [];
  baseUrl = 'http://localhost:5043/';
  currentUserId: string;
  userId: string;

  constructor(private _authService: AuthServiceService, private http: HttpClient, private friendService: FriendService, private route: ActivatedRoute) { }

ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
      this.getAllFriends(this.userId);
    })
}

  getAllFriends(userId: string): void {
    this.isLoading = true;

    this.friendService.getFriendList(userId).subscribe({
      next: (response) => {
        console.log('Data received:', response);
        this.friendList = response.filter(user => user.id !== userId);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Failed to load the data';
        console.error(err);
      }
    });
  }

  onDeleteFriend(friendId: string): void {
  Swal.fire({
    title: 'Are you sure?',
    text: 'You will not be able to recover this friend!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.isConfirmed) {
      this.isLoading = true;
      this.friendService.removeFriend(friendId).subscribe({
        next: (response) => {
          console.log('Data received:', response);
          this.getAllFriends(this.currentUserId); // pass the userId here
          this.isLoading = false;
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Failed to load the data';
          console.error(err);
        }
      });
    }
  });
}


  trackByFriendId(index: number, friend: FriendListDetails): string {
    return friend.id;
  }

}
