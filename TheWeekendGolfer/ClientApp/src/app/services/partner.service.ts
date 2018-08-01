import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Partner } from '../models/partner.model';



@Injectable()
export class PartnerService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string,private _router:Router) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getPartners(playerId: string) {
    return this._http.get(this.theWeekendGolferUrl + 'api/Partner/GetPartners?PlayerId=' + playerId)
      .catch(this.errorHandler);
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
    return this._http.post(this.theWeekendGolferUrl + 'api/Partner/AddPartnerAsync',body,options)
      .catch(this.errorHandler);
  }




  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }

 
}  
