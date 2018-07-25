import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable()
export class CoursesService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: Http, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getCourses() {
    return this._http.get(this.theWeekendGolferUrl + 'api/Course/Index')
      .map((response: Response) => response.json())
      .catch(this.errorHandler);
  }

  getCourseById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Course/Details/" + id)
      .map((response: Response) => response.json())
      .catch(this.errorHandler)
  }


  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }
}  
