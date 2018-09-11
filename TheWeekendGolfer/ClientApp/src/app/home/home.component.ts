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
import { Partner } from '../models/partner.model';
import { Player } from '../models/player.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent {

  public lineChartData: any[] = [];
  public currentHandicaps:any[] = []
  public isLoggedIn = false;
  public playerId = "";
  public partners: Player[] = [];

  constructor(public http: HttpClient, private _router: Router, private _userService: UserService, private _playerService: PlayerService, private _partnerService: PartnerService) {
    this.getHistoricalHandicaps();
    this.checkLogin();
  }


  checkLogin(): void {
    this._userService.isLoggedIn().subscribe(
      data => this.isLoggedIn = data
    )
  }

  getCurrentHandicaps() {
    this._playerService.getPlayerById(this.playerId).subscribe(player => {
      this.currentHandicaps.push({
        name: player.firstName + " " + player.lastName,
        value: Math.round(player.handicap*10)/10
      });
      this.currentHandicaps = [... this.currentHandicaps];
    }
    )
    this.partners.forEach(partner =>
      this._playerService.getPlayerById(partner.id).subscribe(player => {
        this.currentHandicaps.push({
          name: player.firstName + " " + player.lastName,
          value: Math.round(player.handicap * 10) / 10
        });
        this.currentHandicaps = [... this.currentHandicaps];
        console.log(this.currentHandicaps)
        }
        )
      )
  }



  getHistoricalHandicaps() {
    this._userService.getPlayerid().subscribe(
      player => {
        this.playerId = player.id;
        this._playerService.getPlayerHandicaps(player.id).subscribe(
          handicaps => {
            this.lineChartData.push({
              name: player.firstName + " " + player.lastName,
              series: handicaps.map(item => {
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
            this.partners = partners;
            this.getCurrentHandicaps();
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
    domain: ["#4e31a5", "#9c25a7", "#3065ab", "#57468b", "#904497", "#46648b", "#32118d", "#a00fb3", "#1052a2", "#6e51bd", "#b63cc3", "#6c97cb", "#8671c1", "#b455be", "#7496c3"]
  };
}

