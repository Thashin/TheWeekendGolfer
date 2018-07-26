import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { PlayerService } from '../services/player.service'
import { Player } from '../models/player.model'

@Component({
  templateUrl: './player.component.html'
})

export class PlayerComponent {
  public players: Player[];

  constructor(public http: HttpClient, private _router: Router, private _courseService: PlayerService) {
    this.getPlayers();
  }

  getPlayers() {
    this._courseService.getPlayers().subscribe(
      data => this.players = data
    )
  }
}
