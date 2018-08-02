import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { Course } from '../models/course.model';
import { ScoreView } from '../models/scoreView.model';
import { UserService } from '../services/user.service';

@Component({
  templateUrl: './golfRound.component.html'
})

export class GolfRoundComponent {
  public golfRoundViews: GolfRoundView[];

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


}

export class GolfRoundView {
  date: Date;
  course: Course;
  players: ScoreView[];
}
