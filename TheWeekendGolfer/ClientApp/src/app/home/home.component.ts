import { Component, OnInit } from "@angular/core";
import { UserService } from "../services/user.service";
import { PlayerService } from "../services/player.service";
import { PartnerService } from "../services/partner.service";
import { Player, CurrentHandicap } from "../models/player.model";
import { GolfRoundService } from "../services/golfRound.service";
import { AddPartnerDialogComponent } from "../partner/add-partner-dialog.component";
import { MatDialog } from "@angular/material/dialog";
import {
  MatSnackBar,
  MatSnackBarRef,
  SimpleSnackBar
} from "@angular/material/snack-bar";
import { AddGolfRoundDialogComponent } from "../golfRound/add-golfRound-Dialog.component";
import * as _ from 'lodash';
import { forkJoin, Observable } from 'rxjs';
@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"]
})
export class HomeComponent implements OnInit {
  playerHandicapsCount: number;
  playerHandicap: any[];
  lineChartData: any[] = [];
  currentHandicaps: CurrentHandicap[] = [];
  courseStats: any[] = [];
  isLoggedIn = false;
  player: Player;
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
      "#bf9d76",
      "#e99450",
      "#d89f59",
      "#f2dfa7",
      "#a5d7c6",
      "#7794b1",
      "#afafaf",
      "#707160",
      "#ba9383",
      "#d9d5c3"
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
    this.player = undefined;
    this.partners = [];
  }

  openGolfRoundDialog(): void {
    const dialogRef = this.dialog.open(AddGolfRoundDialogComponent, {
      minWidth: "600px"
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
    this.currentHandicaps = [];
    const allPlayers = [...this.partners, this.player];
    const sortedPartners = _.sortBy(allPlayers, player => player.firstName);
    let playerHandicaps$: Observable<Player>[] = [];
    _.forEach(sortedPartners,partner =>
      playerHandicaps$ = [...playerHandicaps$, this._playerService.getPlayerById(partner.id)]
    );
    forkJoin(playerHandicaps$).subscribe((players) => {
      _.forEach(players, player => {
        this.currentHandicaps = [...this.currentHandicaps, {
          name: player.firstName + " " + player.lastName,
          value: Math.round(player.handicap * 10) / 10
        }];
      });
      this.currentHandicaps = _.sortBy(this.currentHandicaps, handicap => handicap.name);

    })
  }

  getCourseStats() {
    this._playerService
      .getPlayerCourseStats(this.player.id)
      .subscribe(courses => {
        for (let name in courses) {
          this.courseStats.push({ name: name, value: courses[name] });
          this.courseStats = [...this.courseStats];
        }
      });
  }

  getPlayerHandicaps(player: Player) {
    this._playerService
      .getPlayerHandicaps(this.player.id)
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
    this._partnerService.getPartners(this.player.id).subscribe(partners => {
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
        this.player = player;
        this.getCourseStats();
        this.getPlayerHandicaps(player);
        this.getPartnerHandicaps();
      } else {
        this._playerService
          .getPlayerByName("Thashin Naidoo")
          .subscribe(thashin => {
            this.player = thashin;
            this.getCourseStats();
            this.getPlayerHandicaps(thashin);
            this.getPartnerHandicaps();
          });
      }
    });
  }
}
