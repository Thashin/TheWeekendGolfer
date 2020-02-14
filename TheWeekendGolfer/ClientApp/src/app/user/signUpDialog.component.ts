import { Component } from "@angular/core";
import { Validators, FormControl } from "@angular/forms";
import { Player } from "../models/player.model";
import { User } from "../models/User.model";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  templateUrl: "./signUpDialog.component.html"
})
export class SignUpDialogComponent {
  player = new Player;
  user = new User;
  firstName: FormControl = new FormControl("", [Validators.required]);
  lastName: FormControl = new FormControl("", [Validators.required]);
  handicap: FormControl = new FormControl("", []);
  email: FormControl = new FormControl("", [
    Validators.required,
    Validators.email
  ]);
  password: FormControl = new FormControl("", [Validators.required]);

  constructor(public dialogRef: MatDialogRef<SignUpDialogComponent>) {}

  getErrorMessage() {
    return this.email.hasError("required")
      ? "You must enter a value"
      : this.email.hasError("email")
      ? "Not a valid email"
      : "";
  }

  signUp() {
    this.dialogRef.close({ player: this.player, user: this.user });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
