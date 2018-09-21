import { Component, Inject, OnInit, AfterViewInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';
import { PlayerService } from '../services/player.service';
import { Score } from '../models/score.model';
import { Player } from '../models/player.model';
import { forEach } from '@angular/router/src/utils/collection';
import { ScoreService } from '../services/scores.service';
import { AddGolfRound } from '../models/addGolfRound.model';
import { UserService } from '../services/user.service';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  templateUrl: './golfRound.component.html'
})

export class AddGolfRoundComponent implements AfterViewInit {


  constructor(private formBuilder: FormBuilder, private _router: Router, private _golfRoundService: GolfRoundService, private _courseService: CourseService, private _playerService: PlayerService, private _scoreService: ScoreService, private _userService: UserService, public dialog: MatDialog) {

    this.createGolfRoundForm = this.formBuilder.group({
      id: [],
      date: ['', Validators.required],
      course: ['', Validators.required],
      tee: ['', Validators.required],
      holes: ['', Validators.required],
      scores: this.formBuilder.array([this.createScore()])
    });
  }

  createScore(): FormGroup {
    return this.formBuilder.group({
      player: '',
      value: ''
    });
  }


  createGolfRoundForm: FormGroup;


  ngAfterViewInit() {
    this.openDialog();

  }

 

  openDialog(): void {

    var scoreData: Score[] = [];
    const dialogRef = this.dialog.open(AddGolfRoundDialog, {
      minWidth: '1000px',
      data: this.createGolfRoundForm
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        for (let score of result.value.scores) {
          var currentScore: Score = {
            playerId: score.player,
            value: score.value
          }
          scoreData.push(currentScore)
        }
        var golfRoundData: AddGolfRound = {
          date: new Date(result.value.date),
          courseId: result.value.holes,
          scores: scoreData
        };
      }
      this._golfRoundService.createGolfRound(golfRoundData).subscribe(data => {
        this._router.navigate(['golf-rounds']);
      });
    });
  }

}

@Component({
  templateUrl: './add-golfRound.component.html',
})
export class AddGolfRoundDialog {
  
  public currentPlayers: Player[];

  public courseNames: string[];
  public holesNames: Course[];
  public tees: string[];
  public courseName: string;

  public allPlayers: Player[];
  public player: Player;
  public scores: FormArray;

  numScores: number;


  constructor(private _courseService: CourseService, private _playerService: PlayerService, private _scoreService: ScoreService, private _userService: UserService,private formBuilder: FormBuilder,
    public dialog: MatDialogRef<AddGolfRoundDialog>,
    @Inject(MAT_DIALOG_DATA) public data: FormArray) {
    console.log(data);
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
    this._playerService.getPlayers().subscribe(data => this.allPlayers = data);
  }

  addPlayer(player: Player) {
    this.currentPlayers.push(player);

  }

  createScore(): FormGroup {
    return this.formBuilder.group({
      player: '',
      value: ''
    });
  }

  addScore(): void {
    this.scores = <FormArray>this.data.get('scores');
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

}
