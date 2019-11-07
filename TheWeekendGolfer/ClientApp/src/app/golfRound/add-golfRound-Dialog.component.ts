import { Component } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormArray,
  FormControl
} from "@angular/forms";
import { Course } from "../models/course.model";
import { CourseService } from "../services/course.service";
import { PlayerService } from "../services/player.service";
import { Player } from "../models/player.model";
import { AddGolfRound } from "../models/addGolfRound.model";
import { UserService } from "../services/user.service";
import { MatDialogRef } from "@angular/material/dialog";

@Component({
  templateUrl: "./add-golfRound-Dialog.component.html",
  styleUrls: ["./add-golfRound-Dialog.component.css"]
})
export class AddGolfRoundDialogComponent {
  currentPlayers: Player[];
  courseNames: string[];
  holesNames: Course[];
  tees: string[];
  courseName: string;
  allPlayers: Player[];
  player: Player;
  numScores: number;
  addGolfRound: AddGolfRound;
  datePlayed: FormControl = new FormControl("", [Validators.required]);
  course = new FormControl("", [Validators.required]);
  tee = new FormControl("", [Validators.required]);
  holes = new FormControl("", [Validators.required]);
  scores = new FormArray([]);


  createScore(): FormGroup {
    return this.formBuilder.group({
      playerId: new FormControl("", [Validators.required]),
      value: new FormControl("", [Validators.required])
    });
  }

  constructor(
    private _courseService: CourseService,
    private _playerService: PlayerService,
    private _userService: UserService,
    private formBuilder: FormBuilder,
    private dialog: MatDialogRef<AddGolfRoundDialogComponent>
  ) {
    this.scores.push(this.createScore());
    this.getCurrentPlayer();
    this.getCourseNames();
    this.getPlayers();
    this.numScores = 1;
    this.currentPlayers = [];
  }
  getCurrentPlayer() {
    this._userService.getPlayerid().subscribe(player => {
      this._playerService.getPlayerById(player.id).subscribe(data => {
        this.player = data;
      });
    });
  }

  getCourseNames() {
    this._courseService
      .getCourseNames()
      .subscribe(data => (this.courseNames = data));
  }

  getCourseTees(courseName: string) {
    this.courseName = courseName;
    this._courseService
      .getCourseTees(courseName)
      .subscribe(data => (this.tees = data));
  }

  getCourseHoles(teeName: string) {
    this._courseService
      .getCourseHoles(this.courseName, teeName)
      .subscribe(data => (this.holesNames = data));
  }

  getPlayers() {
    this._playerService.getPlayers().subscribe(data => {
      this.allPlayers = data;
      this.allPlayers.sort((a, b) => {
        if (a.lastName < b.lastName) return -1;
        else if (a.lastName > b.lastName) return 1;
        else return 0;
      });
    });
  }

  addPlayer(player: Player) {
    this.currentPlayers.push(player);
  }

  addScore(): void {
    this.scores.push(this.createScore());
    this.numScores += 1;
  }

  removeScore(i: number): void {
    this.scores.removeAt(i);
    this.numScores -= 1;
  }

  onNoClick(): void {
    this.dialog.close();
  }

  submitGolfRound(): void {
    this.addGolfRound.scores = [];
    this.scores.controls.forEach(data =>
      this.addGolfRound.scores.push(data.value)
    );
    this.dialog.close(this.addGolfRound);
  }
}
