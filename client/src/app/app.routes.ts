import { FriendRequestListComponent } from './friend-request-list/friend-request-list.component';
import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from './auth.guard';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ProfileComponent } from './profile/profile.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { FriendsListComponent } from './friend-list/friend-list.component';
import { FriendSendListComponent } from './friend-send-list/friend-send-list.component';
import { SideBarComponent } from './side-bar/side-bar.component';
import { ChatComponent } from './chat/chat.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'main-layout',
    pathMatch: 'full',
  },
  {
    path: 'login',
    component: LoginComponent,
    title: 'Login',
  },
  {
    path: 'main-layout',
    component: MainLayoutComponent,
    children: [
      {
        path: '',
        component: WelcomeComponent,
        title: 'Welcome',
      },
      {
        path: 'dashboard',
        component: DashboardComponent,
        canActivate: [AuthGuard],
        title: 'Dashboard',
      },
      {
        path: 'profile/:id',
        component: ProfileComponent,
        canActivate: [AuthGuard],
        title: 'Profile',
      },
      {
        path: 'edit/:id',
        component: EditProfileComponent,
        canActivate: [AuthGuard],
        title: 'Edit Profile',
      },
      {
        path: 'friend-request-list',
        component: FriendRequestListComponent,
        canActivate: [AuthGuard],
        title: 'friend-request-list',
      },
      {
        path: 'friend-list/:id',
        component: FriendsListComponent,
        canActivate: [AuthGuard],
        title: 'friend-list',
      },
      {
        path: 'friend-send-list',
        component: FriendSendListComponent,
        canActivate: [AuthGuard],
        title: 'friend-send-list',
      },
      {
        path: 'chat/:id/:name',
        component: ChatComponent,
        canActivate: [AuthGuard],
        title: 'chat',
      },
    ],
  },
  {
    path: '404',
    component: NotFoundComponent,
    title: 'Page Not Found',
  },
  {
    path: '**',
    redirectTo: '404',
  },
];
