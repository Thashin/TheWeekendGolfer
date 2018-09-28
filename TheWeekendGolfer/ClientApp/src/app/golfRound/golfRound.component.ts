import { Component, Inject, OnInit, ViewChild, AfterViewInit, OnChanges, SimpleChanges, ChangeDetectorRef, AfterContentInit } from '@angular/core';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { Course } from '../models/course.model';
import { ScoreView } from '../models/scoreView.model';
import { UserService } from '../services/user.service';
import { MatPaginator, MatSort, MatTableDataSource, MatDialog, SimpleSnackBar, MatSnackBarRef, MatSnackBar } from '@angular/material';
import { Player } from '../models/player.model';
import { Score } from '../models/score.model';
import { AddGolfRound } from '../models/addGolfRound.model';
import { Router } from '@angular/router';
import { AddGolfRoundDialogComponent } from './add-golfRound-Dialog.component';

@Component({
  templateUrl: './golfRound.component.html',
  styleUrls: ['./golfRound.component.css']
})

export class GolfRoundComponent implements OnInit {


  public golfRoundViews: MatTableDataSource<GolfRoundView>;
  displayedColumns: string[] = ['date', 'course', 'teeName', 'holes', 'par', 'scratchRating', 'slope', 'player1', 'player2', 'player3', 'player4'];
  isLoggedIn: boolean;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private cdRef: ChangeDetectorRef, private snackBar: MatSnackBar, private _router: Router, private _golfRoundService: GolfRoundService, private _userService: UserService, public dialog: MatDialog) {
    _userService.observableIsLoggedIn.subscribe(data => this.isLoggedIn = data);
  }

  ngOnInit() {
    this.isLoggedIn = this._userService.getIsLoggedIn();
    this.getGolfRounds();
  }

  getGolfRounds() {
    this._golfRoundService.getGolfRounds().subscribe(
      data => {
        this.golfRoundViews = new MatTableDataSource(data);
        this.golfRoundViews.paginator = this.paginator;

        this.golfRoundViews.sortingDataAccessor = (item, property) => {
          switch (property) {
            case 'date': return item.date.valueOf();
            case 'course': return item.course.name;
            case 'teeName': return item.course.teeName;
            case 'par': return item.course.par;
            case 'scratchRating': return item.course.scratchRating;
            case 'slope': return item.course.slope;
          }
        }

        this.golfRoundViews.sort = this.sort;
      })
  }

  filterNames(players, filter): boolean {
    var found: boolean = false;
    players.data.forEach(player => {
      if (found) {
        if (players.firstName.trim().toLowerCase().indexOf(filter) !== -1) {
          found = true;
        }
      }
    });
    return found;
  }

  applyFilter(filterValue: string) {
    this.golfRoundViews.filterPredicate = (data, filter) =>
      (
        data.course.name.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.course.holes.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.course.teeName.trim().toLowerCase().indexOf(filter) !== -1
      );


    this.golfRoundViews.filter = filterValue.trim().toLowerCase();

    if (this.golfRoundViews.paginator) {
      this.golfRoundViews.paginator.firstPage();
    }
  }

  openDialog(): void {

    var scoreData: Score[] = [];
    const dialogRef = this.dialog.open(AddGolfRoundDialogComponent, {
      minWidth: '1000px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this._golfRoundService.createGolfRound(result).subscribe(data => {
          if (data) {
            this.openSnackBar("Golf Round Created Successfully");
            this._router.navigate(['golf-rounds']);
          }
          else {
            this.openDialog();
            this.openSnackBar("Unable to create Golf Round" );
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

}




export interface GolfRoundView {
  date: Date;
  course: Course;
  players: ScoreView[];
}
