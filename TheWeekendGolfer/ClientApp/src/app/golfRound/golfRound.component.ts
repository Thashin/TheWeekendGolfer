import { Component, Inject, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { Course } from '../models/course.model';
import { ScoreView } from '../models/scoreView.model';
import { UserService } from '../services/user.service';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Player } from '../models/player.model';

@Component({
  templateUrl: './golfRound.component.html'
})

export class GolfRoundComponent {

  public golfRoundViews: MatTableDataSource<GolfRoundView>;
  displayedColumns: string[] = ['date', 'course', 'teeName', 'holes', 'par', 'scratchRating', 'slope', 'player1', 'player2', 'player3', 'player4'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  isLoggedIn = false;
  constructor(public http: HttpClient, private _router: Router, private _golfRoundService: GolfRoundService, private _userService: UserService) {

    this.getGolfRounds();

    this.checkLogin();
  }


  checkLogin(): void {
    this._userService.isLoggedIn().subscribe(
      data => this.isLoggedIn = data
    )
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
        console.log(this.golfRoundViews);
      })
  }

  filterNames(players, filter): boolean{
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

}


export interface GolfRoundView {
  date: Date;
  course: Course;
  players: ScoreView[];
}
