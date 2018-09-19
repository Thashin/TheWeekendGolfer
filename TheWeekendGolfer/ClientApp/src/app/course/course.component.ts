import { Component, Inject, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { CourseService } from '../services/course.service'
import { Course } from '../models/course.model'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';

@Component({
  selector:'courses',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})

export class CourseComponent{
  public courses: MatTableDataSource<Course>;
  displayedColumns: string[] = ['name', 'location', 'teeName', 'holes', 'par', 'scratchRating', 'slope'];

  @ViewChild(MatPaginator) paginator: MatPaginator;


  constructor(private _courseService: CourseService) {

    this.getCourses();
  }

  @ViewChild(MatSort) sort: MatSort;


  getCourses() {
    this._courseService.getCourses().subscribe(
      data => {
        this.courses = new MatTableDataSource(data);
        this.courses.paginator = this.paginator;
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

