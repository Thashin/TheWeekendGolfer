import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { PlayerService } from "../services/player.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";

@Component({
  selector: "app-add-player",
  templateUrl: "./add-player.component.html"
})
export class AddPlayerComponent implements OnInit {
  createPlayerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private playerService: PlayerService
  ) {}

  ngOnInit() {
    this.createPlayerForm = this.formBuilder.group({
      id: [],
      firstName: ["", Validators.required],
      lastName: ["", Validators.required],
      handicap: ["", Validators.required]
    });
  }

  onSubmit() {
    this.playerService
      .createPlayer(this.createPlayerForm.value)
      .subscribe(data => {
        this.router.navigate(["players"]);
      });
  }
}
