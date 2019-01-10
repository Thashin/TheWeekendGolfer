import { Component, ViewChild, OnInit } from '@angular/core';
import { PlayerService } from '../services/player.service'
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { GolfRoundService } from '../services/golfRound.service';
import { GolfRoundView } from '../golfRound/golfRound.component';
import { CourseStat } from '../models/courseStat.model';

@Component({
  templateUrl: './playerProfile.component.html',
  styleUrls: ['./playerProfile.component.css']
})

export class PlayerProfileComponent implements OnInit {
  
  public playerId;
  public playerName;
  public roundsPlayed: MatTableDataSource<GolfRoundView>
  public courseStats: MatTableDataSource<CourseStat>;
  public displayedColumns: string[] = ['date', 'course', 'teeName', 'holes', 'par', 'scratchRating', 'slope', 'player1'];
  public courseDisplayedColumns: string[] = [ 'course', 'teeName', 'holes', 'count','min', 'average', 'max'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private _golfRoundService: GolfRoundService,private activatedRoute: ActivatedRoute,private _playerService: PlayerService) {

  }

  ngOnInit() {
    this.playerId = this.activatedRoute.snapshot.queryParamMap.get('PlayerId');
    this._playerService.getPlayerById(this.playerId).subscribe(data => {
      this.playerName = data.firstName + " " + data.lastName;
    })
    this._golfRoundService.getGolfRoundCourseAverages(this.playerId).subscribe(
      data => { this.courseStats = new MatTableDataSource(data); console.log(this.courseStats); }
  
    );
    this._golfRoundService.getGolfRoundForPlayerId(this.playerId).subscribe(data => this.roundsPlayed = new MatTableDataSource(data))
  }


}
