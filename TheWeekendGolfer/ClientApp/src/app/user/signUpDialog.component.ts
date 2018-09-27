import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  templateUrl: './signUpDialog.component.html'
})
export class SignUpDialogComponent {

  player: Player = new Player;
  user: User = new User;
  firstName: FormControl = new FormControl('', [Validators.required]);
  lastName: FormControl = new FormControl('', [Validators.required]);
  handicap: FormControl = new FormControl('', [Validators.required]);
  email: FormControl = new FormControl('', [Validators.required]);
  password: FormControl = new FormControl('', [Validators.required]);


  constructor(public dialogRef: MatDialogRef<SignUpDialogComponent>) {
  }

  getErrorMessage() {

  }

  signUp() {
    this.dialogRef.close({ player: this.player, user: this.user });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
