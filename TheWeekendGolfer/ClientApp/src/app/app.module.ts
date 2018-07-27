import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { PlayerComponent } from './player/player.component';
import { CourseComponent } from './course/course.component';
import { CourseService } from './services/course.service';
import { PlayerService } from './services/player.service';
import { GolfRoundService } from './services/golfRound.service';
import { GolfRoundComponent } from './golfRound/golfRound.component';
import { AddPlayerComponent } from './player/add-player.component';
import { AddGolfRoundComponent } from './golfRound/add-golfRound.component';
import { ScoreService } from './services/scores.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    PlayerComponent,
    CourseComponent,
    GolfRoundComponent,
    AddGolfRoundComponent,
    AddPlayerComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'courses', component: CourseComponent },  
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'players', component: PlayerComponent },
      { path: 'add-player', component: AddPlayerComponent },
      { path: 'golf-rounds', component: GolfRoundComponent },
      { path: 'add-golf-round', component: AddGolfRoundComponent },
    ])
  ],
  providers: [CourseService, PlayerService, GolfRoundService, ScoreService],
  bootstrap: [AppComponent]
})
export class AppModule { }
