import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class ScoreService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getScores() {
    return this._http.get(this.theWeekendGolferUrl + 'api/Score/Index')
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  getScoreById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Score/Details/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  addScore(score) {
    return this._http.post(this.theWeekendGolferUrl + 'api/Score/Create', score)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  updateScore(score) {
    return this._http.put(this.theWeekendGolferUrl + 'api/Score/Edit', score)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  deleteScore(id) {
    return this._http.delete(this.theWeekendGolferUrl + "api/Score/Delete/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}  