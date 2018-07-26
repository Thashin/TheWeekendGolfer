import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';

@Component({
  templateUrl: './add-golfRound.component.html'
})

export class AddGolfRoundComponent implements OnInit {

  public courseNames: string[];
  public holesNames: string[];
  public tees: string[];

  constructor(private formBuilder: FormBuilder, private router: Router, private _golfRoundService: GolfRoundService, private _courseService: CourseService) {
    this.getCourseNames();
  }

  getCourseNames() {
    this._courseService.getCourseNames().subscribe(data => this.courseNames = data);
  }

  getCourseHoles(courseName: string) {
    this._courseService.getCourseHoles(courseName).subscribe(data => this.holesNames = data);
  }

  getCourseTees(courseName: string) {
    this._courseService.getCourseTees(courseName).subscribe(data => this.tees = data);
  }

  createGolfRoundForm: FormGroup;


  ngOnInit() {

    this.createGolfRoundForm = this.formBuilder.group({
      id: [],
      date: ['', Validators.required],
      course: ['', Validators.required],
      tee: ['', Validators.required],
      holes: ['', Validators.required],
      score: ['', Validators.required]
    });

  }

  onSubmit() {
    this._golfRoundService.createGolfRound(this.createGolfRoundForm.value)
      .subscribe(data => {
        this.router.navigate(['golf-rounds']);
      });
  }
}
