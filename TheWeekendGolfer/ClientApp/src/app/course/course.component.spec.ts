import { CourseComponent } from "./course.component";
import { ComponentFixture, TestBed } from "@angular/core/testing";
import { CourseService } from "../services/course.service";
import { of } from "rxjs";
import { Component } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatNativeDateModule } from "@angular/material/core";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatDialogModule } from "@angular/material/dialog";
import { MatDividerModule } from "@angular/material/divider";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { MatPaginator, MatPaginatorModule } from "@angular/material/paginator";
import { MatSelectModule } from "@angular/material/select";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatSort, MatSortModule } from "@angular/material/sort";
import { MatStepperModule } from "@angular/material/stepper";
import { MatTableDataSource, MatTableModule } from "@angular/material/table";
import { MatToolbarModule } from "@angular/material/toolbar";
import { FormsModule } from "@angular/forms";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

describe('CourseComponent', () => {
  let sut: ComponentFixture<CourseComponent>;
  let mockCourseService;
  let courses;

  @Component({
    selector: 'app-course',
    template: '<div></div>',
  })
  class FakeCourseComponent {
    // @Output() delete = new EventEmitter();
  }

  beforeEach(() => {
    courses = [
      {
        id: '1',
        location: 'WA',
        name: 'Wembley Golf Course',
        holes: '18',
        teeName: 'Blue Men',
        par: 72,
        scratchRating: 71,
        slope: 126
      },
      {
        id: '2',
        location: 'WA',
        name: 'Point Walter',
        holes: '1-9',
        teeName: 'Blue Men',
        par: 35,
        scratchRating: 33.5,
        slope: 115
      },
    ]
    mockCourseService = jasmine.createSpyObj(['getCourses', 'getCourseNames']);
    TestBed.configureTestingModule({
      declarations: [
        CourseComponent,
        FakeCourseComponent
      ],
      imports: [
        MatButtonModule,
        MatCardModule,
        MatDatepickerModule,
        MatDialogModule,
        MatDividerModule,
        MatIconModule,
        MatInputModule,
        MatListModule,
        MatMenuModule,
        MatNativeDateModule,
        MatPaginatorModule,
        MatSelectModule,
        MatSidenavModule,
        MatSortModule,
        MatTableModule,
        FormsModule,
      ],
      providers: [
        { provide: CourseService, useValue: mockCourseService }
      ]
    });
    sut = TestBed.createComponent(CourseComponent);
  });

  it('should set courses correctly from the service', () => {
    mockCourseService.getCourses.and.returnValue(of(courses))
    sut.detectChanges();

    expect(sut.componentInstance.courses.data.length).toBe(2);
  });

})
