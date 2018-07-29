import { Course } from './course.model';
import { Score } from './score.model';

export class GolfRound {
  date: Date;
  course: string;
  teeName: string;
  holes: string;
  player:string[][]
}
