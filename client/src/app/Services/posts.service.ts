import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { Post } from '../shared/Contracts/Post';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  private baseUrl = 'http://localhost:5043/';

  constructor(private http: HttpClient) { }

  createPost(formData: FormData): Observable<any> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    return this.http.post(`${this.baseUrl}api/Posts`, formData, { headers });
  }

  getAllPosts(params: PostQueryParameters): Observable<PagenatedResult<Post>> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    let queryParams = new HttpParams()
      .set('pageIndex', params.pageIndex.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.search) {
      queryParams = queryParams.set('search', params.search);
    }
    if (params.userId) {
      queryParams = queryParams.set('userId', params.userId);
    }
    return this.http.get<PagenatedResult<Post>>(`${this.baseUrl}api/Posts`, {
      headers,
      params: queryParams
    });
  }

  onLike(postId: number): Observable<any> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.post(`${this.baseUrl}api/Likes/${postId}`, {}, { headers, responseType: 'text' });
  }

  deletePost(postId: number): Observable<any> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.delete(`${this.baseUrl}api/Posts/${postId}`, { headers, responseType: 'text' });
  }

  getPost(postId: number): Observable<Post> {
    const token = localStorage.getItem('jwtToken') || '';
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<Post>(`${this.baseUrl}api/Posts/${postId}`, { headers });
  }

}
