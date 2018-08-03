import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { CourseService } from '../services/course.service'
import { Course } from '../models/course.model'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';

@Component({
  selector:'courses',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})

export class CourseComponent {
  public courses: MatTableDataSource<Course>;
  displayedColumns: string[] = ['name', 'location', 'teeName', 'holes', 'par', 'scratchRating', 'slope'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(public http: HttpClient, private _router: Router, private _courseService: CourseService) {
    this.getCourses();
  }

  getCourses() {
    this._courseService.getCourses().subscribe(
      data => {
        this.courses = new MatTableDataSource(data);
    this.courses.paginator = this.paginator;

    this.courses.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'location': return item.location;
        case 'name': return item.name;
        case 'holes': return item.holes;
        case 'teeName': return item.teeName;
        case 'par': return item.par;
        case 'scratchRating': return item.scratchRating;
        case 'slope': return item.slope;
      }
    }

    this.courses.sort = this.sort;
  })
  }



  applyFilter(filterValue: string) {
    this.courses.filterPredicate = (data, filter) =>
      (
        data.name.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.location.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.holes.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.teeName.trim().toLowerCase().indexOf(filter) !== -1
      );


    this.courses.filter = filterValue.trim().toLowerCase();

    if (this.courses.paginator) {
      this.courses.paginator.firstPage();
    }
  }


  public getCourseNames() {
    return this._courseService.getCourseNames();
  }

}

