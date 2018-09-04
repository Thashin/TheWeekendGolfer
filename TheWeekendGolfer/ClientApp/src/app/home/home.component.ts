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
            this.lineChartData = [{ data: handicaps.map(item => item.currentHandicap).reverse(), label: 'Handicap' }];
            this.lineChartLabels = handicaps.map(item => (new Date(item.date).toLocaleDateString("en-GB", { day: "numeric", month: "short", year: "numeric" }))).reverse();
            console.log(this.lineChartLabels);

          }
        )
      });
    
  }

  // lineChart
  public lineChartOptions: any = {
    scales: {
      yAxes: [{
        display: true,
        ticks: {
          beginAtZero: true   // minimum value will be 0.
        }
      }]
    },
    responsive: true
  };
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
