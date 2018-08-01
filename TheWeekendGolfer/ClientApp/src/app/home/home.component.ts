import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Chart, ChartData, Point } from "chart.js";
import { CourseService } from '../services/course.service';
import { Router } from '@angular/router';
import { Course } from '../models/course.model';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { PlayerService } from '../services/player.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  public playerId: string;
  public courses: Course[];
  public slopesPar: Array<any> =[];
  public pars: number[] = [];

  public lineChartData: Array<any>;
  public lineChartLabels: Array<any>; 


  constructor(public http: HttpClient, private _router: Router, private _userService: UserService, private _playerService: PlayerService, private _courseService: CourseService) {


    this.getHandicaps();
    
  }

  ngOnInit(): void {



  }


  getHandicaps() {
    this._userService.getPlayerid().subscribe(
      playerId => {
        this._playerService.getPlayerHandicaps(playerId).subscribe(
          handicaps => {
            this.lineChartData = [{ data : handicaps.map(item => item.value),label:'Handicap'}];
            this.lineChartLabels = handicaps.map(item => item.date);
            console.log(this.lineChartLabels)
          }
        )
      });
    
  }

  // lineChart
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
  public lineChartType: string = 'line';


  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }

}
