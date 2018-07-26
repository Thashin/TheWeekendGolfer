import { Course } from './course.model';
import { Score } from './score.model';

export class GolfRound {
  id: string;
  date: Date;
  course: Course;
  scores: Score[];
}
