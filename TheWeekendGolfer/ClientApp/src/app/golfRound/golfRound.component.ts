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

export class GolfRoundComponent implements AfterViewInit {

  public golfRoundViews: MatTableDataSource<GolfRoundView>;
  displayedColumns: string[] = ['date', 'course', 'teeName', 'holes', 'par', 'scratchRating', 'slope'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  isLoggedIn = false;
  constructor(public http: HttpClient, private _router: Router, private _golfRoundService: GolfRoundService, private _userService: UserService) {

    this.golfRoundViews = new MatTableDataSource([{
      "date": new Date(2018, 7, 18),
      "course": {
        "id": "00000000-0000-0000-0000-000000000000",
        "location": null,
        "name": "Wembley Golf Complex ",
        "holes": "Tuart 10-18",
        "teeName": "Blue Men",
        "par": 35,
        "scratchRating": 34.00,
        "slope": 117.00,
        "created": "2018-08-03T11:40:19.5255925+08:00"
      }, "players": [{
        "player": {
          "id": "00000000-0000-0000-0000-000000000000",
          "userId": "00000000-0000-0000-0000-000000000000",
          "firstName": "Thashin",
          "lastName": "Naidoo",
          "handicap": -5.44,
          "created": "2018-08-03T11:40:19.5255948+08:00",
          "modified": "0001-01-01T00:00:00"
        }, "score": 37
      }]
    },
    {
      "date": new Date(2018, 7, 19),
      "course": {
        "id": "00000000-0000-0000-0000-000000000000",
        "location": null,
        "name": "Wembley Golf Complex ",
        "holes": "Tuart 10-18",
        "teeName": "Blue Men",
        "par": 35,
        "scratchRating": 34.00,
        "slope": 117.00,
        "created": "2018-08-03T11:40:19.5255925+08:00"
      }, "players": [{
        "player": {
          "id": "00000000-0000-0000-0000-000000000000",
          "userId": "00000000-0000-0000-0000-000000000000",
          "firstName": "Thashin",
          "lastName": "Naidoo",
          "handicap": -5.44,
          "created": "2018-08-03T11:40:19.5255948+08:00",
          "modified": "0001-01-01T00:00:00"
        }, "score": 37
      }]
    }]
    )
    this.getGolfRounds();

    this.checkLogin();
  }

  ngAfterViewInit(): void {


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
        this.golfRoundViews.sort = this.sort;
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
