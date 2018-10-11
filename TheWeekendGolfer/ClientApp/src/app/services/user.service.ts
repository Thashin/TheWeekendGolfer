import { throwError as observableThrowError, Observable, Subject } from 'rxjs';
import { Injectable, Inject, EventEmitter } from '@angular/core';
import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';



import { User } from '../models/User.model';
import { Player } from '../models/player.model';



@Injectable()
export class UserService {
  public observableIsLoggedIn = new Subject<boolean>();
  theWeekendGolferUrl: string = "";
  public isLoggedIn: boolean = false;
  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.theWeekendGolferUrl = baseUrl;
    this.observableIsLoggedIn.asObservable();
    this.observableIsLoggedIn.next(this.isLoggedIn);
  }

  checkIsLoggedIn() {
    return this._http.get<boolean>(this.theWeekendGolferUrl + 'api/User/isLoggedIn');
  }

  logout() {
    return this._http.get<boolean>(this.theWeekendGolferUrl + 'api/User/Logout')
  }

  createUser(user: User) {
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
    return this._http.post(this.theWeekendGolferUrl + 'api/user/LoginAsync', body, options);

  }




  setIsLoggedIn(isLoggedIn) {
    this.isLoggedIn = isLoggedIn;
    this.observableIsLoggedIn.next(this.isLoggedIn);
  }

  getIsLoggedIn() {
    return this.isLoggedIn;
  }

  getPlayerid() {
    return this._http.get<Player>(this.theWeekendGolferUrl + 'api/User/GetPlayer');
  }


  errorHandler(error: Response) {
    console.log(error);
    return observableThrowError(error);
  }


}  
