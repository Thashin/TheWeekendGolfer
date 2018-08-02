import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isLoggedIn = false;


  constructor(private router: Router,private _userService: UserService) {
    this.checkLogin()
  }

  ngOnInit(): void {
    this._userService.getEmitter().subscribe(data => {
      this.isLoggedIn = !this.isLoggedIn
    }
      )
  }


  checkLogin(): void {
    this._userService.isLoggedIn().subscribe(
      data => this.isLoggedIn = data
    )
  }

  collapse() {
    this.isExpanded = false;
  }

  public logout() {
    this._userService.logout();
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
