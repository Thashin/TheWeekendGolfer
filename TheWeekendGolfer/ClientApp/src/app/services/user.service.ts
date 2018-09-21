
import {throwError as observableThrowError,  Observable } from 'rxjs';
import { Injectable, Inject, EventEmitter } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';



import { User } from '../models/User.model';
import { Player } from '../models/player.model';



@Injectable()
export class UserService {

  isLoggedInEvent: EventEmitter<any> = new EventEmitter<any>();
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.theWeekendGolferUrl = baseUrl;
  }

  isLoggedIn() {
    return this._http.get<boolean>(this.theWeekendGolferUrl + 'api/User/isLoggedIn');
  }

  logout() {
    var result = "";
    this._http.get(this.theWeekendGolferUrl + 'api/User/Logout').subscribe(data => {
      result = data["Result"];
      if (data["Result"] == "Logout Successful") {
          this.isLoggedInEvent.emit();
          this.router.navigate(['/']);
        }
      });
    return result;
  }

  createUser(user:User) {
    let options = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json; charset=utf-8' }
      )
    };
    const body = JSON.stringify(
      {
        'Email': user.email,
        'Password': user.password,
        'Player': {
          'FirstName': user.player.firstName,
          'LastName': user.player.lastName,
          'Handicap': user.player.handicap
        }
      });
    return this._http.post(this.theWeekendGolferUrl + 'api/user/CreateAsync', body, options);
  }

  loginUser(user: User) {
    let options = {
      headers: new HttpHeaders(
        { 'Content-Type': 'application/json; charset=utf-8' }
      )
    };
    const body = JSON.stringify(
      {
        'Email': user.email,
        'Password': user.password,
      });
    var result = "";
    this._http.post(this.theWeekendGolferUrl + 'api/user/LoginAsync', body, options)
      .subscribe(data => {
        result = data["Result"];
        if (data["Result"] == "Login Successful") {
          this.isLoggedInEvent.emit();
          this.router.navigate(['/']);

        }
      });
    return result;
  }

  getEmitter() {
    return this.isLoggedInEvent;
  }

  getPlayerid() {
    return this._http.get<Player>(this.theWeekendGolferUrl + 'api/User/GetPlayer');
  }


  errorHandler(error: Response) {
    console.log(error);
    return observableThrowError(error);
  }


}  
