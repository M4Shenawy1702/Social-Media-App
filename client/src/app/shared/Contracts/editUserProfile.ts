import { Gender } from '../enums/Gender';

export interface editUserProfile {
  id: string;
  userName: string;
  email: string;
  displayName: string;
  profilePictureUrl: string;
  coverPhotoUrl: string;
  bio: string;
  dateOfBirth: string;
  phoneNumber: string;
  userAddress: UserAddress;
  gender: Gender;
}
export interface UserAddress {
  city: string;
  street: string;
  country: string;
}
