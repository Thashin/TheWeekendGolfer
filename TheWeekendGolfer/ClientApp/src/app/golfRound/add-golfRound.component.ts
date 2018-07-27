import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service'
import { GolfRound } from '../models/golfRound.model'
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { Course } from '../models/course.model';
import { CourseService } from '../services/course.service';

@Component({
  templateUrl: './add-golfRound.component.html'
})

export class AddGolfRoundComponent implements OnInit {

  public courseNames: string[];
  public holesNames: string[];
  public tees: string[];
  public scores: FormArray;
  public courseName: string;

  constructor(private formBuilder: FormBuilder, private router: Router, private _golfRoundService: GolfRoundService, private _courseService: CourseService) {
    this.getCourseNames();
  }

  getCourseNames() {
    this._courseService.getCourseNames().subscribe(data => this.courseNames = data);
  }


  getCourseTees(courseName: string) {
    this.courseName = courseName;
    this._courseService.getCourseTees(courseName).subscribe(data => this.tees = data);
  }


  getCourseHoles(teeName: string) {
    this._courseService.getCourseHoles(this.courseName,teeName).subscribe(data => this.holesNames = data);
  }

  createGolfRoundForm: FormGroup;


  ngOnInit() {
    this.createGolfRoundForm = this.formBuilder.group({
      id: [],
      date: ['', Validators.required],
      course: ['', Validators.required],
      tee: ['', Validators.required],
      holes: ['', Validators.required],
      scores: this.formBuilder.array([this.createScore()])
    });

  }

  createScore(): FormGroup{
    return this.formBuilder.group({
      Player: '',
      Score: ''
    });
  }

  addScore(): void {
    this.scores = <FormArray>this.createGolfRoundForm.get('scores');
    console.log(this.scores);
    console.log(this.createScore())
    this.scores.push(this.createScore());
  }

  onSubmit() {
    this._golfRoundService.createGolfRound(this.createGolfRoundForm.value)
      .subscribe(data => {
        this.router.navigate(['golf-rounds']);
      });
  }
}
