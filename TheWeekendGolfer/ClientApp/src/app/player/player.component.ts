import { Component, Inject, ViewChild } from '@angular/core';
import { PlayerService } from '../services/player.service'
import { Player } from '../models/player.model'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

@Component({
  templateUrl: './player.component.html'
})

export class PlayerComponent {

  public players: MatTableDataSource<Player> | null;
  displayedColumns: string[] = ['name', 'handicap'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _playerService: PlayerService) {
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

    this._playerService.getPlayers().subscribe(
       data => {
         this.players = new MatTableDataSource(data);
         this.players.paginator = this.paginator;
         this.players.sortingDataAccessor = (player, property) => {
           switch (property) {
             case 'name': return player.lastName.toLowerCase();
             default: return player[property];
           }
         };
         this.players.sort = this.sort;
       }
     );
        }
}
