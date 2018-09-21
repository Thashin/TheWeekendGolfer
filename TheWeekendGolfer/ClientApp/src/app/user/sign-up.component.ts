import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-sign-up',
  templateUrl: './../home/home.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements AfterViewInit {

  createUserForm: FormGroup;
  constructor(private formBuilder: FormBuilder, private router: Router, private _userService: UserService, public dialog: MatDialog) {
  this.createUserForm = this.formBuilder.group({

    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    handicap: ['', Validators.required],
    email: ['', Validators.required],
    password: ['', Validators.required]
  });}


  ngAfterViewInit() {


    this.openDialog();
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(SignUpDialog, {
      minWidth: '400px',
      data: this.createUserForm
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        var player: Player = {
          id: null,
          firstName: result.value.firstName,
          lastName: result.value.lastName,
          handicap: result.value.handicap
        }

        var user: User = {
          email: result.value.email,
          password: result.value.password,
          player: player
        }

        this._userService.createUser(user)
          .subscribe(data => {
            this.router.navigate(['players']);
          });
      }
      else {

        this.router.navigate(['/']);
      }
    });
  }

  }
@Component({
  templateUrl: './sign-up.component.html',
})
export class SignUpDialog {

  constructor(
    public dialog: MatDialogRef<SignUpDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  onNoClick(): void {
    this.dialog.close();
  }

}
