import { Course } from './course.model';
import { Score } from './score.model';

export class AddGolfRound {
  date: Date;
  courseId: string;
  scores: Score[];
}
