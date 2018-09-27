import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { MediaMatcher } from '@angular/cdk/layout';
import { User } from '../models/User.model';
import { MatSnackBarRef, SimpleSnackBar, MatDialog, MatSnackBar } from '@angular/material';
import { LoginDialogComponent } from '../user/loginDialog.component';
import { SignUpDialogComponent } from '../user/signUpDialog.component';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {

  isLoggedIn = false;
  private _mobileQueryListener: () => void;
  mobileQuery: MediaQueryList;

  constructor(changeDetectorRef: ChangeDetectorRef, media: MediaMatcher, private router: Router, private _userService: UserService, private dialog: MatDialog,
    private snackBar: MatSnackBar) {
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

  public logout() {
    this._userService.logout();
  }

  public openLoginDialog(): void {
    let dialogRef = this.dialog.open(LoginDialogComponent, {
      minWidth: '400px'
    });

    dialogRef.afterClosed().subscribe(user => {

      if (user !== null) {
        this._userService.loginUser(user).subscribe(data => {
          if (data["Result"] == "Login Successful") {
            this.isLoggedIn = !this.isLoggedIn;
            this.router.navigate(['/']);
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
          if (data = true) {
            this._userService.loginUser(user).subscribe(login => {
              if (login["Result"] == "Login Successful") {
                this.isLoggedIn = !this.isLoggedIn;
                this.router.navigate(['/']);
                this.openSnackBar("Login Successful");
              }
              else {
                this.openLoginDialog();
                this.openSnackBar("Unable to login");
              }
            })
          }
          else {
            this.openSignUpDialog()
            this.openSnackBar("Unable to SignUp");
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
