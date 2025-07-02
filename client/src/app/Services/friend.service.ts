import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { FriendListDetails } from '../shared/Contracts/FreindRequestDetails';
import { FriendStatus } from "../shared/Contracts/FriendStatus";
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment'; 



interface FriendRequestResponse {
  success: boolean;
  message?: string;
  requestId?: string;
}

@Injectable({
  providedIn: 'root'
})
export class FriendService {
  private readonly baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

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

    getFriendList(userId : string): Observable<FriendListDetails[]> {
      const token = localStorage.getItem('jwtToken');
      if (!token) {
        return throwError(() => new Error('Authentication token not found'));
      }

      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });

      return this.http.get<FriendListDetails[]>(`${this.baseUrl}/api/UserRelationships/get-friends/${userId}`, { headers });
    }

  removeFriend(friendId: string): Observable<FriendRequestResponse> {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.delete<FriendRequestResponse>(`${this.baseUrl}/api/UserRelationships/remove-friend/${friendId}`, { headers });
  }
  cancelRequest(requestId: string): Observable<FriendRequestResponse> {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.delete<FriendRequestResponse>(`${this.baseUrl}/api/UserRelationships/cancel-request/${requestId}`, { headers });
  }
  getFriendStatus(friendId: string): Observable<FriendStatus> {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      return throwError(() => new Error('Authentication token not found'));
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<number>(`${this.baseUrl}/api/UserRelationships/get-friend-status/${friendId}`, { headers })
      .pipe(
        map((statusNumber: number) => {
          if (Object.values(FriendStatus).includes(statusNumber)) {
            return statusNumber as FriendStatus;
          }
          return FriendStatus.None;
        })
      );
  }

}