import { throwError as observableThrowError } from "rxjs";
import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Player } from "../models/player.model";
import { Handicap } from "../models/handicap.model";

@Injectable()
export class PlayerService {
  theWeekendGolferUrl: string;

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }

  getPlayers() {
    return this._http.get<Player[]>(
      this.theWeekendGolferUrl + "api/Player/Index"
    );
  }

  getPlayerHandicaps(playerId: string) {
    return this._http.get<Handicap[]>(
      this.theWeekendGolferUrl +
        "api/Player/GetOrderedHandicaps?PlayerId=" +
        playerId
    );
  }

  getPlayerCourseStats(playerId: string) {
    return this._http.get<{ [id: string]: number }[]>(
      this.theWeekendGolferUrl +
        "api/Player/GetAllPlayerRoundCourses?PlayerId=" +
        playerId
    );
  }

  getPlayerById(id: string) {
    return this._http.get<Player>(
      this.theWeekendGolferUrl + "api/Player/Details?PlayerId=" + id
    );
  }

  getPlayerByName(playerName: string) {
    return this._http.get<Player>(
      this.theWeekendGolferUrl + "api/Player/DetailsByPlayerName?PlayerName=" + playerName
    );
  }

  createPlayer(player: Player) {
    let options = {
      headers: new HttpHeaders({
        "Content-Type": "application/json; charset=utf-8"
      })
    };
    let body;
    if (player.handicap != null) {
      body = JSON.stringify({
        FirstName: player.firstName,
        LastName: player.lastName,
        Handicap: String(player.handicap)
      });
    } else {
      body = JSON.stringify({
        FirstName: player.firstName,
        LastName: player.lastName
      });
    }
    return this._http.post(
      this.theWeekendGolferUrl + "api/Player/Create",
      body,
      options
    );
  }

  updatePlayer(player) {
    return this._http.put(this.theWeekendGolferUrl + "api/Player/Edit", player);
  }

  deletePlayer(id) {
    return this._http.delete(
      this.theWeekendGolferUrl + "api/Player/Delete/" + id
    );
  }

  errorHandler(error: Response) {
    return observableThrowError(error);
  }
}
