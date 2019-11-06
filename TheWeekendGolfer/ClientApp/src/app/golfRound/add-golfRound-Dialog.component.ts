import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';
import { ScoreService } from '../services/scores.service';
import { AddGolfRound } from '../models/addGolfRound.model';
import { UserService } from '../services/user.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  templateUrl: './add-golfRound-Dialog.component.html',
  styleUrls: ['./add-golfRound-Dialog.component.css']
})

export class AddGolfRoundDialogComponent {

  datePlayed: FormControl = new FormControl('', [Validators.required]);
  course = new FormControl('', [Validators.required]);
  tee = new FormControl('', [Validators.required]);
  holes = new FormControl('', [Validators.required]);
  scores = new FormArray([]);

  createScore(): FormGroup {
    return this.formBuilder.group({
      playerId: new FormControl('', [Validators.required]),
      value: new FormControl('', [Validators.required])
    });
  }

  public currentPlayers: Player[];

  public courseNames: string[];
  public holesNames: Course[];
  public tees: string[];
  public courseName: string;

  public allPlayers: Player[];
  public player: Player;

  numScores: number;

  addGolfRound: AddGolfRound = new AddGolfRound;


  constructor(private _courseService: CourseService, private _playerService: PlayerService, private _userService: UserService, private formBuilder: FormBuilder,
    public dialog: MatDialogRef<AddGolfRoundDialogComponent>) {

    this.scores.push(this.createScore());
    this.getCurrentPlayer();
    this.getCourseNames();
    this.getPlayers();;
    this.numScores = 1;
    this.currentPlayers = [];
  }
  getCurrentPlayer() {
    this._userService.getPlayerid().subscribe(player => {
      this._playerService.getPlayerById(player.id).subscribe(data => {
        this.player = data;
      })
    });
  }

  getCourseNames() {
    this._courseService.getCourseNames().subscribe(data => this.courseNames = data);
  }


  getCourseTees(courseName: string) {
    this.courseName = courseName;
    this._courseService.getCourseTees(courseName).subscribe(data => this.tees = data);
  }


  getCourseHoles(teeName: string) {
    this._courseService.getCourseHoles(this.courseName, teeName).subscribe(data => this.holesNames = data);
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
    this.scores.controls.forEach(data => this.addGolfRound.scores.push(data.value));
    this.dialog.close(this.addGolfRound);
  }
}
