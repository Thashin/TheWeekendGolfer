import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class GolfRoundService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getGolfRounds() {
    return this._http.get(this.theWeekendGolferUrl + 'api/GolfRound/Index')
      .catch(this.errorHandler);
  }

  getGolfRoundById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/GolfRound/Details/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  createGolfRound(golfRound) {
    return this._http.post(this.theWeekendGolferUrl + 'api/GolfRound/Create', golfRound)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  updateGolfRound(golfRound) {
    return this._http.put(this.theWeekendGolferUrl + 'api/GolfRound/Edit', golfRound)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  deleteGolfRound(id) {
    return this._http.delete(this.theWeekendGolferUrl + "api/GolfRound/Delete/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}  
