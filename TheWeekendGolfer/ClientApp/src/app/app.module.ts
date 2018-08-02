import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ChartsModule } from 'ng2-charts';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { PlayerComponent } from './player/player.component';
import { CourseComponent } from './course/course.component';
import { CourseService } from './services/course.service';
import { PlayerService } from './services/player.service';
import { GolfRoundService } from './services/golfRound.service';
import { GolfRoundComponent } from './golfRound/golfRound.component';
import { AddPlayerComponent } from './player/add-player.component';
import { AddGolfRoundComponent } from './golfRound/add-golfRound.component';
import { ScoreService } from './services/scores.service';
import { SignUpComponent } from './user/sign-up.component';
import { UserService } from './services/user.service';
import { LogInComponent } from './user/log-in.component';
import { AddPartnerComponent } from './partner/add-partner.component';
import { PartnerService } from './services/partner.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PlayerComponent,
    CourseComponent,
    GolfRoundComponent,
    AddGolfRoundComponent,
    AddPlayerComponent,
    SignUpComponent,
    LogInComponent,
    AddPartnerComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ChartsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'courses', component: CourseComponent },  
      { path: 'players', component: PlayerComponent },
      { path: 'add-player', component: AddPlayerComponent },
      { path: 'golf-rounds', component: GolfRoundComponent },
      { path: 'add-golf-round', component: AddGolfRoundComponent },
      { path: 'sign-up', component: SignUpComponent },
      { path: 'login', component: LogInComponent },
      { path: 'logout', component: LogInComponent },
      { path: 'add-partner', component: AddPartnerComponent }
    ])
  ],
  providers: [CourseService, PlayerService, GolfRoundService, ScoreService, UserService, PartnerService],
  bootstrap: [AppComponent]
})
export class AppModule { }
