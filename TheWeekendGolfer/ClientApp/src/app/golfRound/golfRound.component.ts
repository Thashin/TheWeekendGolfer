import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { Course } from '../models/course.model';
import { ScoreView } from '../models/scoreView.model';
import { UserService } from '../services/user.service';
import { Sort } from '@angular/material';

@Component({
  templateUrl: './golfRound.component.html'
})

export class GolfRoundComponent {
  public golfRoundViews: GolfRoundView[];
  public sortedGolfRoundViews: GolfRoundView[];

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
      data => this.golfRoundViews = data)
  }

  sortData(sort: Sort) {
    const data = this.golfRoundViews.slice();
    if (!sort.active || sort.direction === '') {
      this.golfRoundViews = data;
      return;
    }

    this.golfRoundViews = data.sort((a, b) => {
      const isAsc = sort.direction === 'asc';
      switch (sort.active) {
        case 'date': return compare(a.date, b.date, isAsc);
        case 'course': return compare(a.course, b.course, isAsc);
        case 'teeName': return compare(a.course.teeName, b.course.teeName, isAsc);
        case 'par': return compare(a.course.par, b.course.par, isAsc);
        case 'scratchRating': return compare(a.course.scratchRating, b.course.scratchRating, isAsc);
        case 'slope': return compare(a.course.slope, b.course.slope, isAsc);
        default: return 0;
      }
    });
  }
}

function compare(a, b, isAsc) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}

export interface GolfRoundView {
  date: Date;
  course: Course;
  players: ScoreView[];
}
