import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class PlayerService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getPlayers() {
    return this._http.get(this.theWeekendGolferUrl + 'api/Player/Index')
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  getPlayerById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Player/Details/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  addPlayer(player) {
    return this._http.post(this.theWeekendGolferUrl + 'api/Player/Create', player)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  updatePlayer(player) {
    return this._http.put(this.theWeekendGolferUrl + 'api/Player/Edit', player)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  deletePlayer(id) {
    return this._http.delete(this.theWeekendGolferUrl + "api/Player/Delete/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}  
