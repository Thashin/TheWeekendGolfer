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
