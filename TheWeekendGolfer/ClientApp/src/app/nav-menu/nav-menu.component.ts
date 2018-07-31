import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  constructor(private _userService: UserService) {
    this.checkLogin()
  }

  isExpanded = false;
  isLoggedIn = false;


  checkLogin(): void {
    this._userService.isLoggedIn().subscribe(
      data => this.isLoggedIn = data
    )
  }

  collapse() {
    this.isExpanded = false;
  }

  public logout() {
    this._userService.logout().subscribe()
    this.checkLogin();
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
