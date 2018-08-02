import { Component, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';
import { EventEmitter } from 'events';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html'
})
export class LogInComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private router: Router, private _userService: UserService) { }

  loginForm: FormGroup;

  ngOnInit() {

    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

  }

  onSubmit() {

    var user: User = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password,
      player : null   }

      this._userService.loginUser(user)

    }

  }
