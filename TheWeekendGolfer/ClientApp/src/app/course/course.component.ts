import { Component, ViewChild } from "@angular/core";
import { CourseService } from "../services/course.service";
import { Course } from "../models/course.model";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";

@Component({
  selector: "courses",
  templateUrl: "./course.component.html",
  styleUrls: ["./course.component.css"]
})
export class CourseComponent {
  courses: MatTableDataSource<Course>;
  displayedColumns: string[] = [
    "name",
    "location",
    "teeName",
    "holes",
    "par",
    "scratchRating",
    "slope"
  ];

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(private _courseService: CourseService) {
    this.getCourses();
  }

  getCourses() {
    this._courseService.getCourses().subscribe(data => {
      this.courses = new MatTableDataSource(data);
      this.courses.paginator = this.paginator;
      this.courses.sort = this.sort;
    });
  }

  applyFilter(filterValue: string) {
    this.courses.filterPredicate = (data, filter) =>
      data.name
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1 ||
      data.location
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1 ||
      data.holes
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1 ||
      data.teeName
        .trim()
        .toLowerCase()
        .indexOf(filter) !== -1;

    this.courses.filter = filterValue.trim().toLowerCase();

    if (this.courses.paginator) {
      this.courses.paginator.firstPage();
    }
  }

  public getCourseNames() {
    return this._courseService.getCourseNames();
  }
}
