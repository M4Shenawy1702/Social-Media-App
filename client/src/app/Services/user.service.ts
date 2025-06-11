import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { UserProfile } from '../shared/Contracts/UserProfile';
import { Observable } from 'rxjs';
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';
import { UserQueryParameters } from '../shared/Contracts/UserQueryParameters';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'http://localhost:5043/';

  constructor(private http: HttpClient) { }

  getUserById(userId: string): Observable<UserProfile> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get<UserProfile>(`${this.baseUrl}api/Users/${userId}`, { headers });
  }
  getUsers(params: UserQueryParameters): Observable<PagenatedResult<UserProfile>> {
    const token = localStorage.getItem('jwtToken');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    let httpParams = new HttpParams()
      .set('PageNumber', params.pageIndex.toString())
      .set('PageSize', params.pageSize.toString());

    if (params.searchByName) {
      httpParams = httpParams.set('SearchByName', params.searchByName);
    }

    if (params.userId) {
      httpParams = httpParams.set('UserId', params.userId);
    }

    return this.http.get<PagenatedResult<UserProfile>>(`${this.baseUrl}api/Users`, {
      headers,
      params: httpParams
    });
  }
}
