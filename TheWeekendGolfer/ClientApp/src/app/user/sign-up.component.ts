import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { UserService } from '../services/user.service';
import { Player } from '../models/player.model';
import { User } from '../models/User.model';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html'
})
export class SignUpComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private router: Router, private userService: UserService) { }

  createUserForm: FormGroup;

  ngOnInit() {

    this.createUserForm = this.formBuilder.group({

      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      handicap: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required]
    });

  }

  onSubmit() {

    var player: Player = {
      id : null, 
      firstName : this.createUserForm.value.firstName,
      lastName : this.createUserForm.value.lastName,
      handicap : this.createUserForm.value.handicap
    }

    var user: User = {
      email : this.createUserForm.value.email,
      password : this.createUserForm.value.password,
      player : player
    }

      this.userService.createUser(user)
        .subscribe(data => {
          this.router.navigate(['players']);
        });
    }

  }
