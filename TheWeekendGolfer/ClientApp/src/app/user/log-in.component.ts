import { Component, OnInit, Output, Inject, AfterViewInit, OnChanges, AfterContentChecked, AfterContentInit, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-log-in',
  templateUrl: './../home/home.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent implements AfterContentInit {

  loginForm: FormGroup;
  @Output() loggedIn: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private router: Router,private formBuilder: FormBuilder,  private _userService: UserService, public dialog: MatDialog) {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required])
    });
  }

  ngAfterContentInit() {

    this.openDialog();
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(LoginDialog, {
      minWidth: '400px',
      data: this.loginForm
    });

    dialogRef.afterClosed().subscribe(result => {

      if (result != null) {
        var user: User = {
          email: result.value.email,
          password: result.value.password,
          player: null
        }
        this._userService.loginUser(user);

        
      }
      this.router.navigate(['/']);
    });
  }

  getErrorMessage() {
    return this.loginForm.controls['email'].hasError('required') ? 'You must enter a value' :
      this.loginForm.controls['email'].hasError('email') ? 'Not a valid email' :
        '';
  }

}

  @Component({
    templateUrl: './log-in.component.html',
  })  
  export class LoginDialog {

    constructor(
    public dialog: MatDialogRef <LoginDialog> ,
    @Inject(MAT_DIALOG_DATA) public data:User) { }

    onNoClick(): void {
      this.dialog.close();
  }

}
