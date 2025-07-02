import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PostComment } from '../shared/Contracts/PostComment';
import { environment } from '../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class CommentsService {
  private readonly baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }
  addComment(formData: FormData): Observable<PostComment> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.post<PostComment>(`${this.baseUrl}/api/Comments`, formData, { headers });
  }
  deleteComment(commentId: number): Observable<any> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.delete(`${this.baseUrl}/api/Comments/${commentId}`, { headers, responseType: 'text' });
  }

  updateComment(commentId: number, formData: FormData): Observable<PostComment> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.put<PostComment>(`${this.baseUrl}/Comments/${commentId}`, formData, { headers });
  }

}
