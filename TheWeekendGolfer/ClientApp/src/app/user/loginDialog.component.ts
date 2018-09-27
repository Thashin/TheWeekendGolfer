import { Component, OnInit, Output, Inject, AfterViewInit, OnChanges, AfterContentChecked, AfterContentInit, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
/*
@Component({
  selector: 'app-log-in',
  templateUrl: './../home/home.component.html',
  styleUrls: ['./log-in.component.css']
})
export class LogInComponent{


  @Output() loggedIn: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private router: Router,private formBuilder: FormBuilder,  private _userService: UserService, public dialog: MatDialog) {
  }

}*/

  @Component({
    templateUrl: './loginDialog.component.html',
  })  
  export class LoginDialogComponent {
    
  email: FormControl =  new FormControl('', [Validators.required, Validators.email]);
  password: FormControl = new FormControl('', [Validators.required]);
    user: User = new User; 
    constructor(
      public dialogRef: MatDialogRef<LoginDialogComponent>) {

    }

    getErrorMessage() {
      return this.email.hasError('required') ? 'You must enter a value' :
        this.email.hasError('email') ? 'Not a valid email' :
          '';
    }

    login() {
      console.log(this.user);
        this.dialogRef.close(this.user);
    }

    onNoClick(): void {
      this.dialogRef.close();
  }

}
