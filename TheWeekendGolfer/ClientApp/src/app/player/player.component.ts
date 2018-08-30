import { Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { PlayerService } from '../services/player.service'
import { Player } from '../models/player.model'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';

@Component({
  templateUrl: './player.component.html'
})

export class PlayerComponent {

  public players: MatTableDataSource<Player>;
  displayedColumns: string[] = ['name', 'handicap'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(public http: HttpClient, private _router: Router, private _courseService: PlayerService) {
    this.getPlayers();
  }


  applyFilter(filterValue: string) {
    this.players.filterPredicate = (data, filter) =>
      (
        data.firstName.trim().toLowerCase().indexOf(filter) !== -1 ||
        data.lastName.trim().toLowerCase().indexOf(filter) !== -1 
      );


    this.players.filter = filterValue.trim().toLowerCase();

    if (this.players.paginator) {
      this.players.paginator.firstPage();
    }
  }

  getPlayers() {
    this._courseService.getPlayers().subscribe(
      data => {
        this.players = new MatTableDataSource(data);
        this.players.paginator = this.paginator;
        this.players.sort = this.sort;
      }
    )
  }
}
