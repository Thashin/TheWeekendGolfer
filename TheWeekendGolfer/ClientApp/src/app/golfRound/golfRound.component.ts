import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { GolfRoundService } from '../services/golfRound.service'
import { Course } from '../models/course.model';
import { ScoreView } from '../models/scoreView.model';
import { UserService } from '../services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { SimpleSnackBar, MatSnackBarRef, MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Score } from '../models/score.model';
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
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: false}) sort: MatSort;

  constructor(private snackBar: MatSnackBar, private _golfRoundService: GolfRoundService, private _userService: UserService, public dialog: MatDialog) {
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
    players.data.forEach(() => {
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

    const dialogRef = this.dialog.open(AddGolfRoundDialogComponent, {
      minWidth: '1000px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this._golfRoundService.createGolfRound(result).subscribe(data => {
          if (data) {
            this.openSnackBar("Golf Round Created Successfully");
            this.getGolfRounds();
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
