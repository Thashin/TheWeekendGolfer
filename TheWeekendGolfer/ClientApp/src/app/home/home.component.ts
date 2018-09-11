import { Component, AfterViewInit, OnInit } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
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
  styleUrls:['./home.component.css']
})
export class HomeComponent {

  public playerId: string;
  public courses: Course[];
  public slopesPar: Array<any> =[];
  public pars: number[] = [];
  public lineChartData: any[] = [];
  public isLoggedIn = false;
  view: any[] = [500, 300];


  constructor(public http: HttpClient, private _router: Router, private _userService: UserService, private _playerService: PlayerService, private _partnerService: PartnerService) {
    this.getHandicaps();
    this.checkLogin();
  }


  checkLogin(): void {
    this._userService.isLoggedIn().subscribe(
      data => this.isLoggedIn = data
    )
  }


  getHandicaps() {
    this._userService.getPlayerid().subscribe(
      player => {
        this._playerService.getPlayerHandicaps(player.id).subscribe(
          handicaps => {
            this.lineChartData.push({
              name: player.firstName + " " + player.lastName, series: handicaps.map(item => {
                return {
                  name: new Date(item.date),
                  value: item.currentHandicap
                }
              })
            });

            this.lineChartData = [... this.lineChartData];
          });
        this._partnerService.getPartners(player.id).subscribe(
          partners => {
            partners.forEach(
              partner => {
                this._playerService.getPlayerHandicaps(partner.id).subscribe(
                  handicaps => {

                    this.lineChartData.push({
                      name: partner.firstName + " " + partner.lastName, series: handicaps.map(item => {

                        return {
                          name: new Date(item.date),
                          value: item.currentHandicap
                        }
                      })
                    })

                    this.lineChartData = [... this.lineChartData];
                    console.log(this.lineChartData)
                  }
                );
              });
          });
      });

  }

  showXAxis = true;
  showYAxis = true;
  gradient = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Date';
  showYAxisLabel = true;
  yAxisLabel = 'Handicap';
  timeline = true;
  autoScale = true;
  colorScheme = {
    domain: ['#1D68FB', '#33C0FC', '#4AFFFE', '#AFFFFF', '#FFFC63', '#FDBD2D', '#FC8A25', '#FA4F1E', '#FA141B', '#BA38D1']
  };
}

