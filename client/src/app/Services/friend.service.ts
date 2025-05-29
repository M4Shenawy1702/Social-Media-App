import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { FriendListDetails } from '../shared/Contracts/FreindRequestDetails';


interface FriendRequestResponse {
  success: boolean;
  message?: string;
  requestId?: string;
}

@Injectable({
  providedIn: 'root'
})
export class FriendService {
  private readonly baseUrl = 'http://localhost:5043/api';

  constructor(private http: HttpClient) {}

  addFriend(initiatorId: string, receiverId: string): Observable<FriendRequestResponse> {
    // Validate inputs
    if (!receiverId || !initiatorId) {
      return throwError(() => new Error('Invalid user IDs provided'));
    }

    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    return this.http.post<FriendRequestResponse>(
      `${this.baseUrl}/UserRelationships/send-request/${initiatorId}/${receiverId}`,
      {}, // Empty body
      { headers }
    ).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Failed to send friend request';
        if (error.status === 404) {
          errorMessage = 'User not found';
        } else if (error.status === 409) {
          errorMessage = 'Friend request already exists';
        } else if (error.status === 403) {
          errorMessage = 'Not authorized to perform this action';
        }
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  getFriendList(userId: string): Observable<FriendListDetails[]> {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<FriendListDetails[]>(`${this.baseUrl}/UserRelationships/get-friends/${userId}`, { headers });
  }

  removeFriend(friendId: string): Observable<FriendRequestResponse> {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.delete<FriendRequestResponse>(`${this.baseUrl}/UserRelationships/remove-friend/${friendId}`, { headers });
  }
}