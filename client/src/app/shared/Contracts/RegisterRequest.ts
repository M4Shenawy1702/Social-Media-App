import { Gender } from '../enums/Gender'; 

export interface RegisterRequest {
  Email: string;
  UserName: string;
  Password: string;
  ConfirmPassword: string;
  DisplayName: string;
  PhoneNumber: string;
  DateOfBirth: string;
  Gender: Gender | null;
  Bio: string;
  ProfilePicture: File | null;
  CoverPhoto: File | null;
  City: string;
  Street: string;
  Country: string;
}