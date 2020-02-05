import { Player } from "./player.model";

export class Forecast {
  player: Player;
  average: number;
  pb: number;
  hs: number;
  toLowerHandicap: number;
  rangeOfPredictedHandicaps: [{[ id:number]:  number }];
}
