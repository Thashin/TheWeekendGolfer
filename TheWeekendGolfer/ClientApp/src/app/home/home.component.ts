import { Component, AfterViewInit, OnInit } from '@angular/core';
import { Chart, ChartData, Point } from "chart.js";
import { CourseService } from '../services/course.service';
import { Router } from '@angular/router';
import { Course } from '../models/course.model';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { PlayerService } from '../services/player.service';
import { PartnerService } from '../services/partner.service';
import { forEach } from '@angular/router/src/utils/collection';
import { a } from '@angular/core/src/render3';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  public playerId: string;
  public courses: Course[];
  public slopesPar: Array<any> =[];
  public pars: number[] = [];

  
  public lineChartData: Array<any> = new Array<any>();
  public lineChartLabels: Array<any>;

  constructor(public http: HttpClient, private _router: Router, private _userService: UserService, private _playerService: PlayerService, private _partnerService: PartnerService) {


    this.getHandicaps();
    
  }

  ngOnInit(): void {
    this.lineChartData.push({ data:[46.61, 41.12, 37.47, 35.64, 35.64, 31.98, 33.81, 33.81, 35.03, 35.03, 36.41, 36.41, 37.35, 36.88, 36.67, 34.73, 33.55, 32.55],label:"1cbdcc08-cb61-4ec1-799a-08d61143260b" });
    this.lineChartData.push({ data: [18, 48, 77, 9, 100, 27, 40], label: 'Series C' });


  }


  getHandicaps() {
    this._userService.getPlayerid().subscribe(
      playerId => {
        this._playerService.getPlayerHandicaps(playerId).subscribe(
          handicaps => {
            var data: number[] = handicaps.map(item => item.currentHandicap).reverse();
            console.log(data);
            this.lineChartData.push({ data: data , label: 'Handicap' });
            this.lineChartLabels = handicaps.map(item => item.date).reverse();
          }
        );
        this._partnerService.getPartners(playerId).subscribe(
          partners => {
            partners.forEach(
              partner => {
                this._playerService.getPlayerHandicaps(partner).subscribe(
                  handicaps => {

                    var data: number[] = handicaps.map(item => item.currentHandicap).reverse();
                    //this.lineChartData.push({ data: data, label: partner });
                    this.lineChartLabels.push(handicaps.map(item => item.date).reverse());
                  }
                );
            });
          });
        console.log(this.lineChartData);
      });
  }

  // lineChart
  public lineChartOptions: any = {
    scales: {
      xAxes: [{
        type: 'time',
        time: {
          unit: 'day',
          displayFormats: {
            'day': 'DD MMM'
          }
        }
      }],
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
