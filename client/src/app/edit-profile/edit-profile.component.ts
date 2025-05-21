import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProfile } from '../shared/Contracts/UserProfile'; 
import { Gender } from '../shared/enums/Gender'; 


@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.scss']
})
export class EditProfileComponent implements OnInit {

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) {}

  baseUrl = 'http://localhost:5043/';
  userId: string = '';
  
  userProfile: UserProfile = {
    id: '',
    userName: '',
    email: '',
    displayName: '',
    profilePictureUrl: '',
    coverPhotoUrl: '',
    bio: '',
    dateOfBirth: '',
    createdAt: '',
    isOnline: false,
    phoneNumber: '',
    gender: Gender.Male,
    userAddress: {
      city: '',
      street: '',
      country: ''
    }
  };

  profilePictureFile: File | null = null;
  coverPhotoFile: File | null = null;
  coverPreview: string | ArrayBuffer | null = null;
  profilePreview: string | ArrayBuffer | null = null;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.userId = params['id'];
      this.getUserProfile();
    });
  }

  getUserProfile() {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      console.error('No token found');
      this.router.navigate(['/login']);
      return;
    }

    const headers = {
      'Authorization': `Bearer ${token}`
    };

    this.http.get<UserProfile>(`${this.baseUrl}api/users/${this.userId}`, { headers }).subscribe({
      next: (profile) => {
        this.userProfile = profile;
      },
      error: (err) => {
        console.error('Error loading profile:', err);
        if (err.status === 401) {
          this.router.navigate(['/login']);
        }
      }
    });
  }

  updateUserProfile(model: UserProfile) {
    const token = localStorage.getItem('jwtToken');
    if (!token) {
      console.error('No token found');
      return;
    }
  
    const headers = {
      'Authorization': `Bearer ${token}`
    };
  
    const formData = new FormData();
  
    formData.append('UserName', model.userName);
    formData.append('Email', model.email);
    formData.append('DisplayName', model.displayName);
    formData.append('Bio', model.bio);
    formData.append('DateOfBirth', model.dateOfBirth);
    formData.append('Gender', model.gender.toString());
    formData.append('PhoneNumber', '0123456789'); 
  
    formData.append('City', model.userAddress.city);
    formData.append('Street', model.userAddress.street);
    formData.append('Country', model.userAddress.country);
  
    if (this.profilePictureFile) {
      formData.append('ProfilePicture', this.profilePictureFile);
    }
  
    if (this.coverPhotoFile) {
      formData.append('CoverPhoto', this.coverPhotoFile); 
    }
  
    this.http.put(`${this.baseUrl}api/users/update/${this.userId}`, formData, { headers })
      .subscribe({
        next: (response) => {
          this.router.navigate(['/main-layout/profile', this.userId]);
        },
        error: (err) => { 
          console.error('Error updating profile:', err);
          console.log('Error details:', err.error);
        }
      });
  }

  onProfilePhotoChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.profilePictureFile = file;
      const reader = new FileReader();
      reader.onload = () => this.profilePreview = reader.result;
      reader.readAsDataURL(file);
    }
  }

  onCoverPhotoChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.coverPhotoFile = file;
      const reader = new FileReader();
      reader.onload = () => this.coverPreview = reader.result;
      reader.readAsDataURL(file);
    }
  }

}
