import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PostComment } from '../shared/Contracts/PostComment';

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private baseUrl = 'http://localhost:5043/';

  constructor(private http: HttpClient) { }
  addComment(formData: FormData): Observable<PostComment> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.post<PostComment>(`${this.baseUrl}api/Comments`, formData, { headers });
  }
  deleteComment(commentId: number): Observable<any> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.delete(`${this.baseUrl}api/Comments/${commentId}`, { headers, responseType: 'text' });
  }

  updateComment(commentId: number, formData: FormData): Observable<PostComment> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.put<PostComment>(`${this.baseUrl}api/Comments/${commentId}`, formData, { headers });
  }

}
