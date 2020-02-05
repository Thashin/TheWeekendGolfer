import { Component, OnInit } from "@angular/core";
import { UserService } from "../services/user.service";
import { PlayerService } from "../services/player.service";
import { PartnerService } from "../services/partner.service";
import { Player } from "../models/player.model";
import { GolfRoundService } from "../services/golfRound.service";
import { AddPartnerDialogComponent } from "../partner/add-partner-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import {
  MatSnackBar,
  MatSnackBarRef,
  SimpleSnackBar
} from "@angular/material/snack-bar";
import { AddGolfRoundDialogComponent } from "../golfRound/add-golfRound-Dialog.component";
import { ForecastDialogComponent } from "../forecast/forecast-Dialog.component";
@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit {
  playerHandicapsCount: number;
  playerHandicap: any[];
  lineChartData: any[] = [];
  currentHandicaps: any[] = [];
  courseStats: any[] = [];
  isLoggedIn = false;
  playerId = "";
  partners: Player[] = [];

  showXAxis = true;
  showYAxis = true;
  gradient = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = "Date";
  showYAxisLabel = true;
  yAxisLabel = "Handicap";
  timeline = true;
  autoScale = true;
  colorScheme = {
    domain: [
      '#bf9d76', '#e99450', '#d89f59', '#f2dfa7', '#a5d7c6', '#7794b1', '#afafaf', '#707160', '#ba9383', '#d9d5c3'
    ]
  };

  constructor(
    private _golfRoundService: GolfRoundService,
    private _userService: UserService,
    private _playerService: PlayerService,
    private _partnerService: PartnerService,
    private snackBar: MatSnackBar,
    public dialog: MatDialog
  ) {
    _userService.observableIsLoggedIn.subscribe(data => {
      this.isLoggedIn = data;
      this.getHistoricalHandicaps();
    });
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
    this.getHistoricalHandicaps();
  }

  openPartnerDialog(): void {
    const dialogRef = this.dialog.open(AddPartnerDialogComponent, {
      minWidth: "1000px"
    });

    dialogRef.afterClosed().subscribe(partner => {
      if (partner != null) {
        this._partnerService.addPartner(partner).subscribe(data => {
          if (data) {
            this.openSnackBar("Partner Created Successfully");
            this.getHistoricalHandicaps();
          } else {
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
    const dialogRef = this.dialog.open(AddGolfRoundDialogComponent, {
      minWidth: "1000px"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this._golfRoundService.createGolfRound(result).subscribe(data => {
          if (data) {
            this.openSnackBar("Golf Round Created Successfully");
            this.getHistoricalHandicaps();
          } else {
            this.openGolfRoundDialog();
            this.openSnackBar("Unable to create Golf Round");
          }
        });
      }
    });
  }

  openForecastDialog(): void {
    const dialogRef = this.dialog.open(ForecastDialogComponent, {
      minWidth: "1000px"
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this._golfRoundService.createGolfRound(result).subscribe(data => {
          if (data) {
            this.openSnackBar("Golf Round Created Successfully");
            this.getHistoricalHandicaps();
          } else {
            this.openGolfRoundDialog();
            this.openSnackBar("Unable to create Golf Round");
          }
        });
      }
    });
  }

  openSnackBar(message: string): MatSnackBarRef<SimpleSnackBar> {
    return this.snackBar.open(message, "", {
      duration: 5000
    });
  }

  getCurrentHandicaps() {
    this._playerService.getPlayerById(this.playerId).subscribe(player => {
      this.currentHandicaps.push({
        name: player.firstName + " " + player.lastName,
        value: Math.round(player.handicap * 10) / 10
      });
      this.currentHandicaps = [...this.currentHandicaps];
    });
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
        this.currentHandicaps = [...this.currentHandicaps];
      })
    );
  }

  getCourseStats() {
    this._playerService
      .getPlayerCourseStats(this.playerId)
      .subscribe(courses => {
        for (let name in courses) {
          this.courseStats.push({ name: name, value: courses[name] });
          this.courseStats = [...this.courseStats];
        }
      });
  }

  getPlayerHandicaps(player: Player) {
    this._playerService
      .getPlayerHandicaps(this.playerId)
      .subscribe(handicaps => {
        this.lineChartData.push({
          name: player.firstName + " " + player.lastName,
          series: handicaps.map(item => {
            return {
              name: new Date(item.date),
              value: item.currentHandicap
            };
          })
        });
        this.playerHandicapsCount = handicaps.length;
        this.lineChartData = [...this.lineChartData];
      });
  }

  getPartnerHandicaps() {
    this._partnerService.getPartners(this.playerId).subscribe(partners => {
      this.partners = partners;
      this.getCurrentHandicaps();
      partners.forEach(partner => {
        this._playerService
          .getPlayerHandicaps(partner.id)
          .subscribe(handicaps => {
            this.lineChartData.push({
              name: partner.firstName + " " + partner.lastName,
              series: handicaps.map(item => {
                return {
                  name: new Date(item.date),
                  value: item.currentHandicap
                };
              })
            });
            this.lineChartData = this.lineChartData.sort((a, b) => {
              if (a.name < b.name) return -1;
              else if (a.name > b.name) return 1;
              else return 0;
            });
            this.lineChartData = [...this.lineChartData];
          });
      });
    });
  }

  getHistoricalHandicaps() {
    this._userService.getPlayerid().subscribe(player => {
      this.resetStats();
      if (player) {
        this.playerId = player.id;
        this.getCourseStats();
        this.getPlayerHandicaps(player);
        this.getPartnerHandicaps();
        console.log(this.lineChartData)
      } else {
        this._playerService.getPlayerByName("Thashin Naidoo").subscribe(thashin => {
          this.playerId = thashin.id;
          this.getCourseStats();
          this.getPlayerHandicaps(thashin);
          this.getPartnerHandicaps();
        });
      }
    });
  }
}
