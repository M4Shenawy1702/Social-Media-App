import { Component } from '@angular/core';
import { AuthServiceService } from '../Services/AuthService/auth-service.service';
import { NavComponent } from "../nav/nav.component";
import { RouterOutlet } from '@angular/router';
import { SideBarComponent } from "../side-bar/side-bar.component";

@Component({
  selector: 'app-main-layout',
  standalone: true, 
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss'],
  imports: [NavComponent, RouterOutlet, SideBarComponent]
})
export class MainLayoutComponent {
  constructor(private authService: AuthServiceService) {}

  logout(): void {
    this.authService.logout();
  }
}