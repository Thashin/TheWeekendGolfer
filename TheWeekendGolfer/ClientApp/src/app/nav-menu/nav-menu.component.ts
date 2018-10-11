import { Component, OnInit, ChangeDetectorRef, OnDestroy, Output } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { MediaMatcher } from '@angular/cdk/layout';
import { User } from '../models/User.model';
import { MatSnackBarRef, SimpleSnackBar, MatDialog, MatSnackBar } from '@angular/material';
import { LoginDialogComponent } from '../user/loginDialog.component';
import { SignUpDialogComponent } from '../user/signUpDialog.component';
import { EventEmitter } from 'events';
import { PlayerService } from '../services/player.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  public isLoggedIn = false;
  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;

  constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private router: Router, private _userService: UserService, private _playerService: PlayerService, private dialog: MatDialog,
    private snackBar: MatSnackBar) {
    _userService.observableIsLoggedIn.subscribe(data => this.isLoggedIn = data);
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
  }
  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }



  public logout() {
    this._userService.logout().subscribe(data => {
      if (data == true) {
        this._userService.setIsLoggedIn(false);
        this.router.navigate(['/'])
      }
    });
  }

  public openLoginDialog(): void {
    let dialogRef = this.dialog.open(LoginDialogComponent, {
      minWidth: '400px'
    });

    dialogRef.afterClosed().subscribe(user => {

      if (user != null) {
        this._userService.loginUser(user).subscribe(data => {
          if (data["Result"] == "Login Successful") {
            this._userService.setIsLoggedIn(true);
          }
          else {
            this.openLoginDialog();
          }
          this.openSnackBar(data["Result"]);
        });
      }
    });
  }

  public openSignUpDialog(): void {
    let dialogRef = this.dialog.open(SignUpDialogComponent, {
      minWidth: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        var user: User = {
          email: result.user.email,
          password: result.user.password,
          player: result.player
        }
        this._userService.createUser(user).subscribe(data => {
          if (data == true) {
            this._userService.loginUser(user).subscribe(login => {
              if (login["Result"] == "Login Successful") {
                this._userService.setIsLoggedIn(true);
                this._playerService.createPlayer(user.player).subscribe(data => {
                  if (data) {
                    this.router.navigate(["/"]);
                    this.openSnackBar("Signup Successful");
                  }
                });
              }
              else {
                this.openLoginDialog();
                this.openSnackBar("Unable to Login");
              }
            })
          }
          else {
            this.openSignUpDialog()
            this.openSnackBar(data.toString());
          }
        }

        )

      }
    });
  }

  openSnackBar(message: string): MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, "", {
      duration: 5000,
    });
  }

}
