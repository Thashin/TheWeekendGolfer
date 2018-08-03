import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { MediaMatcher } from '@angular/cdk/layout';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  isLoggedIn = false;
  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;

  constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private router: Router,private _userService: UserService) {
    this.checkLogin()
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnInit(): void {
    this._userService.getEmitter().subscribe(data => {
      this.isLoggedIn = !this.isLoggedIn
    }
      )
  }
  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
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
