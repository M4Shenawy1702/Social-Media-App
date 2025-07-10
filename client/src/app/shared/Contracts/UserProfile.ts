import { Gender } from '../enums/Gender';

export interface UserProfile {
  id: string;
  userName: string;
  email: string;
  displayName: string;
  profilePictureUrl: string;
  coverPhotoUrl: string;
  bio: string;
  dateOfBirth: string;
  isOnline: boolean;
  phoneNumber: string;
  userAddress: UserAddress;
  gender: Gender;
  friendsCount: number
}
export interface UserAddress {
  city: string;
  street: string;
  country: string;
}
