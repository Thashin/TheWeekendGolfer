import { Component, Inject, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';
import { forEach } from '@angular/router/src/utils/collection';
import { UserService } from '../services/user.service';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef, MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { Forecast } from '../models/forecast.model';
import { ForecastService } from '../services/forecast.service';

@Component({
  templateUrl: './forecast-Dialog.component.html',
  styleUrls: ['./forecast-Dialog.component.css']
})

export class ForecastDialogComponent {

  datePlayed: FormControl = new FormControl('', [Validators.required]);
  course = new FormControl('', [Validators.required]);
  tee = new FormControl('', [Validators.required]);
  holes = new FormControl('', [Validators.required]);
  scores = new FormArray([]);


  public currentPlayers: Player[];

  public courseNames: string[];
  public holesNames: Course[];
  public tees: string[];
  public courseName: string;

  public forecasts: MatTableDataSource<Forecast>;
  displayedColumns: string[] = ['playerName','average','personalBest','highestScore','toLowerHandicap','sixty','fiftyFive','fifty','fortyFive','forty','thirtyFive'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort
  
  numScores: number;

  forecast: Forecast = new Forecast;


  constructor(private _courseService: CourseService, private _forecastService: ForecastService, private _userService: UserService, private formBuilder: FormBuilder,
    public dialog: MatDialogRef<ForecastDialogComponent>) {
    this.getCourseNames();
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

  getForecasts(courseId: string) {
    this._forecastService.getForecasts(courseId).subscribe(
      data =>
      {
        console.log(data);
        this.forecasts = new MatTableDataSource(data);
        this.forecasts.paginator = this.paginator;

        this.forecasts.sortingDataAccessor = (item, property) => {
          switch (property) {
            case 'playerName': return item.player.firstName.valueOf();
        }
      }

        this.forecasts.sort = this.sort;
    });

  }





  onNoClick(): void {
    this.dialog.close();
  }


}
