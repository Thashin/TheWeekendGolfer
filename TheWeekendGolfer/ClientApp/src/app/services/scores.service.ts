import { throwError as observableThrowError } from "rxjs";
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class ScoreService {
  theWeekendGolferUrl: string;

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }

  getScores() {
    return this._http.get(this.theWeekendGolferUrl + "api/Score/Index");
  }

  getScoreById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Score/Details/" + id);
  }

  updateScore(score) {
    return this._http.put(this.theWeekendGolferUrl + "api/Score/Edit", score);
  }

  deleteScore(id) {
    return this._http.delete(
      this.theWeekendGolferUrl + "api/Score/Delete/" + id
    );
  }

  errorHandler(error: Response) {
    return observableThrowError(error);
  }
}
