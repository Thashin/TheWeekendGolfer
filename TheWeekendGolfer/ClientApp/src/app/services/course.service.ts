import { Observable } from 'rxjs';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Course } from '../models/course.model';

@Injectable()
export class CourseService {
  theWeekendGolferUrl: string = "";

  constructor(private _http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _router: Router) {
    this.theWeekendGolferUrl = baseUrl;
  }

  getCourses() {
    return this._http.get<Course[]>(this.theWeekendGolferUrl + 'api/Course/Index');
  }


  getCourseNames() {
    return this._http.get<string[]>(this.theWeekendGolferUrl + 'api/Course/GetCourseNames');
  }

  getCourseTees(courseName: string) {
    return this._http.get<string[]>(this.theWeekendGolferUrl + 'api/Course/GetCourseDetails?CourseName=' + courseName);
  }
  getCourseHoles(courseName: string, courseTee: string) {
    return this._http.get<Course[]>(this.theWeekendGolferUrl + 'api/Course/GetCourseDetails?CourseName=' + courseName + "&Tee=" + courseTee);
  }

  getCourseById(id: string) {
    return this._http.get<Course>(this.theWeekendGolferUrl + "api/Course/Details/" + id);
  }

}  
