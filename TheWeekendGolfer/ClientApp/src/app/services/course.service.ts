import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { Course } from '../models/course.model';



@Injectable()
export class CourseService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }


  getCourses(){
    return this._http.get(this.theWeekendGolferUrl + 'api/Course/Index')
      .catch(this.errorHandler);
  }

  getCourseById(id: string) {
    return this._http.get(this.theWeekendGolferUrl + "api/Course/Details/" + id)
      .catch(this.errorHandler)
  }


  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }

 
}  