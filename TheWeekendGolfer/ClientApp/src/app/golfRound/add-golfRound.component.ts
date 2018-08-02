import { Component, Inject, OnInit } from '@angular/core';
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

@Component({
  templateUrl: './add-golfRound.component.html'
})

export class AddGolfRoundComponent implements OnInit {

  public courseNames: string[];
  public holesNames: Course[];
  public tees: string[];
  public scores: FormArray;
  public allPlayers: Player[];
  public currentPlayers: Player[];
  public courseName: string;
  public player: Player;
  numScores: number;

  constructor(private formBuilder: FormBuilder, private router: Router, private _golfRoundService: GolfRoundService, private _courseService: CourseService, private _playerService: PlayerService, private _scoreService: ScoreService, private _userService: UserService) {
    this.getCurrentPlayer();
    this.getCourseNames();
    this.getPlayers();
    this.numScores = 1;
  }

  getCurrentPlayer() {
    this._userService.getPlayerid().subscribe(playerId => {
      this._playerService.getPlayerById(playerId).subscribe(data => {
        this.player = data;
        console.log(this.player);
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

  createGolfRoundForm: FormGroup;


  ngOnInit() {
    this.currentPlayers = [];
    this.createGolfRoundForm = this.formBuilder.group({
      id: [],
      date: ['', Validators.required],
      course: ['', Validators.required],
      tee: ['', Validators.required],
      holes: ['', Validators.required],
      scores: this.formBuilder.array([this.createScore()])
    });

  }

  addPlayer(player: Player) {
    this.currentPlayers.push(player);
    
  }

  createScore(): FormGroup{
    return this.formBuilder.group({
      player:'',
      value: ''
    });
  }

  addScore(): void {
    this.scores = <FormArray>this.createGolfRoundForm.get('scores');
    this.scores.push(this.createScore());
    this.numScores += 1;
  }

  removeScore(i:number): void {
    this.scores.removeAt(i);
    this.numScores -= 1;
  }

  onSubmit()
  {

    var scoreData: Score[] = [];

    for (let score of this.createGolfRoundForm.value.scores) {
      var currentScore: Score = {
        playerId: score.player,
        value: score.value
      }
      scoreData.push(currentScore)
    }
    var golfRoundData: AddGolfRound = {
      date: new Date(this.createGolfRoundForm.value.date),
      courseId: this.createGolfRoundForm.value.holes,
      scores: scoreData
    };
    this._golfRoundService.createGolfRound(golfRoundData).subscribe(data => {
        this.router.navigate(['golf-rounds']);
      });
  }
}
