import { Course } from './course.model';
import { Score } from './score.model';
import { Player } from './player.model';

export class GolfRound {
  id: string;
  date: Date;
  name: string;
  teeName: string;
  holes: string;
  player: Player;
  value: number;
}
