import { Component, AfterViewInit } from '@angular/core';
import { Chart, ChartData, Point } from "chart.js";
import { CourseService } from '../services/course.service';
import { Router } from '@angular/router';
import { Course } from '../models/course.model';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent{

  public courses: Course[];
  public slopesPar: Array<any> =[];
  public pars: number[] = [];

  constructor(public http: HttpClient, private _router: Router, private _courseService: CourseService) {
    this.getCoursesOrderedSlope();
  }

  getCoursesOrderedSlope() {
    this._courseService.getCoursesOrderedSlope().subscribe(
      data => {
        for (let course of data) {
          
            this.slopesPar.push({ x: course.par, y: course.slope })
        }
        console.log(this.slopesPar)
      }
    )
  }


  // lineChart
  public lineChartLabels: Array<any> = this.pars;
  public lineChartOptions: any = {
    responsive: true
  };
  public lineChartColors: Array<any> = [
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend: boolean = true;
  public lineChartType: string = 'scatter';


  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

}
