import * as _ from "lodash";
import { Component } from "@angular/core";
import { Validators, FormArray, FormControl } from "@angular/forms";
import { Course } from "../models/course.model";
import { CourseService } from "../services/course.service";
import { Player } from "../models/player.model";
import { MatDialogRef } from "@angular/material/dialog";
import { Forecast } from "../models/forecast.model";
import { ForecastService } from "../services/forecast.service";

@Component({
  templateUrl: "./forecast-Dialog.component.html",
  styleUrls: ["./forecast-Dialog.component.css"]
})
export class ForecastDialogComponent {
  currentPlayers: Player[];
  courseNames: string[];
  holesNames: Course[];
  tees: string[];
  courseName: string;
  numScores: number;
  forecast: Forecast;

  lineChartData: any[] = [];
  showXAxis = true;
  showYAxis = true;
  gradient = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = "Score";
  showYAxisLabel = true;
  yAxisLabel = "Handicap";
  timeline = true;
  autoScale = true;
  colorScheme = {
    domain: [
      '#bf9d76', '#e99450', '#d89f59', '#f2dfa7', '#a5d7c6', '#7794b1', '#afafaf', '#707160', '#ba9383', '#d9d5c3'
    ]
  };

  datePlayed: FormControl = new FormControl("", [Validators.required]);
  course = new FormControl("", [Validators.required]);
  tee = new FormControl("", [Validators.required]);
  holes = new FormControl("", [Validators.required]);
  scores = new FormArray([]);

  constructor(
    private _courseService: CourseService,
    private _forecastService: ForecastService,
    private dialog: MatDialogRef<ForecastDialogComponent>
  ) {
    this.getCourseNames();
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

  getForecasts(courseId: string) {
    this._forecastService.getForecasts(courseId).subscribe(forecasts => {
     _.forEach(forecasts, forecast => {
        this.lineChartData.push({
          name: forecast.player.firstName + " " + forecast.player.lastName,
          series: _.map(
            forecast.rangeOfPredictedHandicaps,
            (handicap, score) => {
              return {
                name: Number(score),
                value: handicap
              };
            }
          )
        });
      });
      console.log(this.lineChartData.length);
    });
  }

  onNoClick(): void {
    this.dialog.close();
  }
}
