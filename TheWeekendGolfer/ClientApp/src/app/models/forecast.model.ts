import { Course } from './course.model';
import { Score } from './score.model';
import { Player } from './player.model';

export class Forecast {
  player: Player;
  average: number;
  pb: number;
  hs: number;
  toLowerHandicap: number;
  sixty: number;
  fiftyFive: number;
  fifty: number;
  fortyFive: number;
  forty: number;
  thirtyFive: number;
}
