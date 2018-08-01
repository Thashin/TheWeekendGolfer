import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Player } from '../models/player.model';

@Injectable()
export class PlayerService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getPlayers() {
    return this._http.get(this.theWeekendGolferUrl + 'api/Player/Index')
      .catch(this.errorHandler);
  }

  getPlayerHandicaps(playerId: string) {
    return this._http.get(this.theWeekendGolferUrl + 'api/Player/GetOrderedHandicaps?PlayerId=' + playerId)
      .catch(this.errorHandler);
  }

  getPlayerById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Player/Details/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }

  createPlayer(player: Player) {
    let options = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json; charset=utf-8' }
      )
    };
    const body = JSON.stringify(
      {
        'FirstName': player.firstName,
        'LastName': player.lastName,
        'Handicap': String(player.handicap)
      });
    return this._http.post(this.theWeekendGolferUrl + 'api/Player/Create', body, options)
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
