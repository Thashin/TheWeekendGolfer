import { throwError as observableThrowError } from "rxjs";
import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";
import { Player } from "../models/player.model";

@Injectable()
export class PartnerService {
  theWeekendGolferUrl: string;

  constructor(
    private _http: HttpClient,
    @Inject("BASE_URL") baseUrl: string,
    private _router: Router
  ) {
    this.theWeekendGolferUrl = baseUrl;
  }

  getPartners(playerId: string) {
    return this._http.get<Player[]>(
      this.theWeekendGolferUrl + "api/Partner/GetPartners?PlayerId=" + playerId
    );
  }

  getPotentialPartners(playerId: string) {
    return this._http.get<Player[]>(
      this.theWeekendGolferUrl +
        "api/Partner/GetPotentialPartners?PlayerId=" +
        playerId
    );
  }

  addPartner(partnerId: string) {
    let options = {
      headers: new HttpHeaders({
        "Content-Type": "application/json; charset=utf-8"
      })
    };
    const body = JSON.stringify({
      PartnerId: partnerId
    });
    return this._http.post(
      this.theWeekendGolferUrl + "api/Partner/AddPartnerAsync",
      body,
      options
    );
  }

  removePartner(partnerId: string) {
    let options = {
      headers: new HttpHeaders({
        "Content-Type": "application/json; charset=utf-8"
      })
    };
    const body = JSON.stringify({
      PartnerId: partnerId
    });
    return this._http.post(
      this.theWeekendGolferUrl + "api/Partner/RemovePartnerAsync",
      body,
      options
    );
  }

  errorHandler(error: Response) {
    return observableThrowError(error);
  }
}
