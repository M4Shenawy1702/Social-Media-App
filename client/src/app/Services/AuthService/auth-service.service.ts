import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthServiceService {
  private isAuthenticated = false;
  private user: any = null; // هنا لتخزين بيانات المستخدم عند تسجيل الدخول

  baseurl = 'http://localhost:5043/api/Authentication/';

  constructor(private http: HttpClient, private router: Router) {
    this.isAuthenticated = localStorage.getItem('isLoggedIn') === 'true';
    if (this.isAuthenticated) {
      this.loadUserData(); // تحميل بيانات المستخدم من الـ localStorage
    }
  }

  login(loginModel: { Email: string; Password: string }) {
    return this.http.get(`${this.baseurl}login?Email=${loginModel.Email}&Password=${loginModel.Password}`);
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

  // دالة لتحميل بيانات المستخدم من localStorage
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
    return this.http.post(`${this.baseurl}register`, formData);
  }
}
