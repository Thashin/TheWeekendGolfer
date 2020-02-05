import { Component } from "@angular/core";
import { Validators, FormControl } from "@angular/forms";
import { User } from "../models/User.model";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  templateUrl: "./loginDialog.component.html"
})
export class LoginDialogComponent {
  email = new FormControl("", [Validators.required, Validators.email]);
  password = new FormControl("", [Validators.required]);
  user: User = new User;

  constructor(private dialogRef: MatDialogRef<LoginDialogComponent>) {}

  getErrorMessage() {
    return this.email.hasError("required")
      ? "You must enter a value"
      : this.email.hasError("email")
      ? "Not a valid email"
      : "";
  }

  login() {
    this.dialogRef.close(this.user);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
