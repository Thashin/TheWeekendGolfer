import { Component, ViewChild } from '@angular/core';
import { Validators, FormArray, FormControl } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';
import { Player } from '../models/player.model';
import { MatDialogRef } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
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
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: false}) sort: MatSort
  
  numScores: number;

  forecast: Forecast = new Forecast;


  constructor(private _courseService: CourseService, private _forecastService: ForecastService, public dialog: MatDialogRef<ForecastDialogComponent>) {
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
