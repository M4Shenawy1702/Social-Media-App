import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment'; 


@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private isAuthenticated = false;
  private user: any = null;

  private baseUrl = environment.baseUrl; 

  constructor(private http: HttpClient, private router: Router) {
    this.isAuthenticated = localStorage.getItem('isLoggedIn') === 'true';
    if (this.isAuthenticated) {
      this.loadUserData();
    }
  }

  login(loginModel: { Email: string; Password: string }) {
    return this.http.get(`${this.baseUrl}/api/Authentication/login?Email=${loginModel.Email}&Password=${loginModel.Password}`);
  }

  logout(): void {
    this.isAuthenticated = false;
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return this.isAuthenticated || localStorage.getItem('isLoggedIn') === 'true';
  }

  private loadUserData() {
    const userData = localStorage.getItem('user');
    if (userData) {
      this.user = JSON.parse(userData);
    }
  }

  getCurrentUserId(): string {
    const userJson = localStorage.getItem('user');
    if (!userJson) return '';

    try {
      const user = JSON.parse(userJson);
      return user?.userId || '';
    } catch {
      return '';
    }
  }

  registerWithFormData(formData: FormData) {
    return this.http.post(`${this.baseUrl}/api/Authentication/register`, formData);
  }
}
