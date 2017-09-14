import { AuthService } from './../../services/auth.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './navmenu.component.html',
  styleUrls: ['./navmenu.component.scss']
})
export class NavMenuComponent {

  constructor(private authService: AuthService) { }

  onLogoutClick() {
    this.authService.logout().subscribe();
  }
}
