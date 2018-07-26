import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseService } from '../services/course.service'
import { Course } from '../models/course.model'

@Component({
  templateUrl: './course.component.html'
})

export class CourseComponent {
  public courses: Course[];

  constructor(public http: HttpClient, private _router: Router, private _courseService: CourseService) {
    this.getCourses();
  }

  getCourses() {
    this._courseService.getCourses().subscribe(
      data => this.courses = data
    )
  }

}

