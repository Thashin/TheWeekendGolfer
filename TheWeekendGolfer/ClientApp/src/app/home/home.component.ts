import { Component, AfterViewInit, OnInit, Input, OnChanges, ChangeDetectorRef, Output } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course.model';
import { UserService } from '../services/user.service';
import { PlayerService } from '../services/player.service';
import { PartnerService } from '../services/partner.service';
import { Partner } from '../models/partner.model';
import { Player } from '../models/player.model';
import { GolfRoundService } from '../services/golfRound.service';
import { ActivatedRoute } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AddPartnerDialogComponent } from '../partner/add-partner-dialog.component';
import { MatSnackBar, MatDialog, MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { Score } from '../models/score.model';
import { AddGolfRoundDialogComponent } from '../golfRound/add-golfRound-Dialog.component';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {


  public playerHandicapsCount: number;
  public playerHandicap: any[];
  public lineChartData: any[] = [];
  public currentHandicaps: any[] = [];
  public courseStats: any[] = [];
  public isLoggedIn = false;
  public playerId = "";
  public partners: Player[] = [];

  constructor(private _golfRoundService: GolfRoundService, private _userService: UserService, private _playerService: PlayerService, private _partnerService: PartnerService, private snackBar: MatSnackBar, public dialog: MatDialog) {
    _userService.observableIsLoggedIn.subscribe(data => { this.isLoggedIn = data; this.resetStats(); this.getHistoricalHandicaps(); });
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
    this.getHistoricalHandicaps();
  }

  openPartnerDialog(): void {
    const dialogRef = this.dialog.open(AddPartnerDialogComponent, {
      minWidth: '1000px',
    });

    dialogRef.afterClosed().subscribe(partner => {
      if (partner != null) {
        this._partnerService.addPartner(partner).subscribe(data => {
          if (data) {
            this.openSnackBar("Partner Created Successfully");
            this.resetStats();
            this.getHistoricalHandicaps();
          }
          else {
            this.openPartnerDialog();
            this.openSnackBar("Unable to create Partner");
          }
        });
      }
    });
  }
  resetStats(): void {
    this.lineChartData = [];
    this.currentHandicaps = [];
    this.courseStats = [];
    this.playerHandicapsCount = 0;
    this.playerHandicap = [];
    this.playerId = "";
    this.partners = [];
  }

  openGolfRoundDialog(): void {

    var scoreData: Score[] = [];
    const dialogRef = this.dialog.open(AddGolfRoundDialogComponent, {
      minWidth: '1000px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this._golfRoundService.createGolfRound(result).subscribe(data => {
          if (data) {
            this.openSnackBar("Golf Round Created Successfully");
            this.resetStats();
            this.getHistoricalHandicaps();
          }
          else {
            this.openGolfRoundDialog();
            this.openSnackBar("Unable to create Golf Round");
          }
        });
      }
    });
  }

  openSnackBar(message: string): MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, "", {
      duration: 5000,
    });
  }

  getCurrentHandicaps() {
    this._playerService.getPlayerById(this.playerId).subscribe(player => {
      this.currentHandicaps.push({
        name: player.firstName + " " + player.lastName,
        value: Math.round(player.handicap * 10) / 10
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
        this.currentHandicaps = this.currentHandicaps.sort((a, b) => {
          if (a.value < b.value) return -1;
          else if (a.value > b.value) return 1;
          else return 0;
        });
        this.currentHandicaps = [... this.currentHandicaps];
      }
      )
    )
  }

  getCourseStats() {
    this._playerService.getPlayerCourseStats(this.playerId).subscribe(courses => {
      for (let name in courses) {
        this.courseStats.push({ name: name, value: courses[name] })
        this.courseStats = [... this.courseStats]
      }
    });
  }

  getPlayerHandicaps(player: Player) {
    this._playerService.getPlayerHandicaps(this.playerId).subscribe(
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
        this.playerHandicapsCount = handicaps.length;
        this.lineChartData = [... this.lineChartData];
      });
  }

  getPartnerHandicaps() {
    this._partnerService.getPartners(this.playerId).subscribe(
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
                this.lineChartData = this.lineChartData.sort((a, b) => {
                  if (a.name < b.name) return -1;
                  else if (a.name > b.name) return 1;
                  else return 0;
                });
                this.lineChartData = [... this.lineChartData];
              }
            );
          });
      });
  }

  getHistoricalHandicaps() {
    this._userService.getPlayerid().subscribe(
      player => {
        if (player) {
          this.playerId = player.id;
          this.getCourseStats();
          this.getPlayerHandicaps(player);
          this.getPartnerHandicaps();
        }
        else {
          this.playerId = '2d566298-4d3d-42d8-589e-08d62e8da6fb'
          this._playerService.getPlayerById(this.playerId).subscribe(thashin => {
            this.getCourseStats();
            this.getPlayerHandicaps(thashin);
            this.getPartnerHandicaps();
          }
          );
        }
      }
    );
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
