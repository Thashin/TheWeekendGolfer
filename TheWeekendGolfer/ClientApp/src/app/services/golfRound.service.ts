
import { throwError as observableThrowError, Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GolfRound } from '../models/golfRound.model';
import { AddGolfRound } from '../models/addGolfRound.model';
import { GolfRoundView } from '../golfRound/golfRound.component';
import { CourseStat } from '../models/courseStat.model';

@Injectable()
export class GolfRoundService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getGolfRounds() {
    return this._http.get<GolfRoundView[]>(this.theWeekendGolferUrl + 'api/GolfRound/Index');
  }

  getGolfRoundById(id: string) {
    return this._http.get<GolfRound>(this.theWeekendGolferUrl + "api/GolfRound/Details/" + id);
  }

  getGolfRoundForPlayerId(id: string) {
    return this._http.get<GolfRoundView[]>(this.theWeekendGolferUrl + "api/GolfRound/Details?PlayerId=" + id);
  }

  getGolfRoundCourseAverages(id: string) {
    return this._http.get<CourseStat[]>(this.theWeekendGolferUrl + "api/GolfRound/CourseAverages?PlayerId=" + id);
  }

  createGolfRound(golfRound: AddGolfRound) {
    let options = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json; charset=utf-8' }
      )
    };
    const body = JSON.stringify(
      {
        'Date': golfRound.date,
        'CourseId': golfRound.courseId,
        'Scores': golfRound.scores
      });
    return this._http.post(this.theWeekendGolferUrl + 'api/GolfRound/Create', body, options);
  }

  updateGolfRound(golfRound) {
    return this._http.put(this.theWeekendGolferUrl + 'api/GolfRound/Edit', golfRound);
  }

  deleteGolfRound(id) {
    return this._http.delete(this.theWeekendGolferUrl + "api/GolfRound/Delete/" + id);
  }

  errorHandler(error: Response) {
    console.log(error);
    return observableThrowError(error);
  }
}  
