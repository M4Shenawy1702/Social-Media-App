import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { CommonModule } from '@angular/common';
import { RegisterRequest } from '../shared/Contracts/RegisterRequest';
import { LoginModel } from '../shared/Contracts/LoginModel';

export enum Gender {
  Male = 'Male',
  Female = 'Female'
}


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  isloginview: boolean = true;
  isLoggedIn: boolean = false;
  genderOptions = Object.values(Gender);
  profilePicturePreview: string | ArrayBuffer | null = null;
  coverPhotoPreview: string | ArrayBuffer | null = null;

  RegisterRequest: RegisterRequest = {
    Email: '',      
    UserName: '',    
    Password: '',  
    ConfirmPassword: '',  
    DisplayName: '', 
    PhoneNumber: '', 
    DateOfBirth: '', 
    Gender: null, 
    Bio: '', 
    ProfilePicture: null, 
    CoverPhoto: null, 
    City: '', 
    Street: '', 
    Country: '', 
  };

  loginModel: LoginModel = {
    Email: '',
    Password: ''
  };

  constructor(
    private router: Router,
    private _httpClient: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object,
    private _authService: AuthServiceService
  ) {}

  // دالة معالجة تحميل الملفات
  handleFileInput(event: Event, field: 'ProfilePicture' | 'CoverPhoto'): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      this.RegisterRequest[field] = file;

      // إنشاء معاينة للصورة
      const reader = new FileReader();
      reader.onload = (e) => {
        if (field === 'ProfilePicture') {
          this.profilePicturePreview = e.target?.result as string;
        } else {
          this.coverPhotoPreview = e.target?.result as string;
        }
      };
      reader.readAsDataURL(file);
    }
  }

loginUser(loginModel: LoginModel) {
  this._authService.login(loginModel).subscribe({
    next: (data: any) => {
      this.isLoggedIn = true;

      localStorage.setItem('isLoggedIn', 'true');
      localStorage.setItem('user', JSON.stringify(data));

      // Save the JWT token separately, assuming it's in data.token
      if (data.token) {
        localStorage.setItem('jwtToken', data.token);
      } else {
        console.warn('Token not found in login response');
      }

      this.router.navigate(['/main-layout']);
    },
    error: (err) => {
      console.error('Login failed', err);
      alert('Login failed. Please check your credentials.');
    }
  });
}

  
  registerUser() {
    if (this.isFormValid()) {
      if (this.RegisterRequest.Password !== this.RegisterRequest.ConfirmPassword) {
        alert('Passwords do not match!');
        return;
      }

      // إنشاء FormData لإرسال البيانات
      const formData = new FormData();

      // إضافة جميع الحقول إلى FormData
      Object.keys(this.RegisterRequest).forEach(key => {
        const value = this.RegisterRequest[key as keyof RegisterRequest];
        if (value !== null && value !== undefined) {
          if (value instanceof File) {
            formData.append(key, value, value.name);
          } else {
            formData.append(key, value.toString());
          }
        }
      });

      this._authService.registerWithFormData(formData).subscribe({
        next: (res: any) => {
          alert('Registration successful!');
          this.isloginview = true;
          this.resetRegisterForm();
        },
        error: (err) => {
          console.error('Registration error:', err);
        
          if (err.error && err.error.errors) {
            const validationErrors = err.error.errors;
            let message = 'Registration failed due to the following errors:\n';
            for (const field in validationErrors) {
              if (validationErrors.hasOwnProperty(field)) {
                message += `${field}: ${validationErrors[field].join(', ')}\n`;
              }
            }
            alert(message);
          } else {
            alert('Registration failed. Please try again.');
          }
        }        
      });
    } else {
      alert('Please fill in all required fields.');
    }
  }
  
  isFormValid(): boolean {
    return (
      !!this.RegisterRequest.Email &&          
      !!this.RegisterRequest.UserName &&       
      !!this.RegisterRequest.Password &&       
      !!this.RegisterRequest.ConfirmPassword &&
      !!this.RegisterRequest.DisplayName &&    
      !!this.RegisterRequest.PhoneNumber &&
      !!this.RegisterRequest.Gender
    );
  }

  private resetRegisterForm(): void {
    this.RegisterRequest = {
      Email: '',      
      UserName: '',    
      Password: '',  
      ConfirmPassword: '',  
      DisplayName: '', 
      PhoneNumber: '', 
      DateOfBirth: '', 
      Gender: null, 
      Bio: '', 
      ProfilePicture: null, 
      CoverPhoto: null, 
      City: '', 
      Street: '', 
      Country: '', 
    };
    this.profilePicturePreview = null;
    this.coverPhotoPreview = null;
  }
}