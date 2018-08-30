import { Component, OnInit, Output, Inject, AfterViewInit, OnChanges, AfterContentChecked, AfterContentInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
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
export class LogInComponent implements AfterViewInit {

  loginForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private _userService: UserService, public dialog: MatDialog) {
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngAfterViewInit() {

    this.openDialog();
  }


  openDialog(): void {
    const dialogRef = this.dialog.open(LoginDialog, {
      minWidth: '400px',
      data: this.loginForm
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(result);
      var user: User = {
        email: result.value.email,
        password: result.value.password,
        player: null
      }
      this._userService.loginUser(user)
    });
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
