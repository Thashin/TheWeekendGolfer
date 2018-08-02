
import {throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';



import { Partner } from '../models/partner.model';



@Injectable()
export class PartnerService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string,private _router:Router) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getPartners(playerId: string) {
    return this._http.get<Partner[]>(this.theWeekendGolferUrl + 'api/Partner/GetPartners?PlayerId=' + playerId);
  }

  addPartner(partner: Partner) {
    let options = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json; charset=utf-8' }
      )
    };
    const body = JSON.stringify(
      {
        'PartnerId': partner.partnerId
      });
    return this._http.post(this.theWeekendGolferUrl + 'api/Partner/AddPartnerAsync', body, options);
  }




  errorHandler(error: Response) {
    console.log(error);
    return observableThrowError(error);
  }

 
}  
