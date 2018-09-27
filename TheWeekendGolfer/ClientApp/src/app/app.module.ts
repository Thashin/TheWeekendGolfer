import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {
  MatButtonModule,
  MatMenuModule,
  MatToolbarModule,
  MatIconModule,
  MatCardModule, 
  MatTableModule,
  MatStepperModule,
  MatDatepickerModule,
  MatSortModule,
  MatDialogModule,
  MatDividerModule,
  MatInputModule,
  MatListModule,
  MatSelectModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatSidenavModule,
  MatSnackBar,
  MatSnackBarModule} from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {FlexLayoutModule} from '@angular/flex-layout';
import { NgxChartsModule } from '@swimlane/ngx-charts';
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
import { AddGolfRoundComponent, AddGolfRoundDialog } from './golfRound/add-golfRound.component';
import { ScoreService } from './services/scores.service';
import { SignUpComponent, SignUpDialog } from './user/sign-up.component';
import { UserService } from './services/user.service';
import { AddPartnerComponent, AddPartnerDialog } from './partner/add-partner.component';
import { PartnerService } from './services/partner.service';
import { PartnerComponent } from './partner/partner.component';
import { AboutComponent } from './about/about.component';
import { LoginDialogComponent } from './user/loginDialog.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PlayerComponent,
    PartnerComponent,
    CourseComponent,
    GolfRoundComponent,
    AddGolfRoundComponent,
    AddGolfRoundDialog,
    AddPlayerComponent,
    AddPartnerDialog,
    SignUpComponent,
    SignUpDialog,
    LoginDialogComponent,
    AddPartnerComponent,
    AboutComponent
  ],
  entryComponents: [LoginDialogComponent, SignUpDialog, AddGolfRoundDialog, AddPartnerDialog],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpClientXsrfModule.withOptions({
      cookieName: 'XSRF-TOKEN',
      headerName: 'X-CSRF-TOKEN'
    }),
    BrowserAnimationsModule,
    FlexLayoutModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatSelectModule,
    MatSidenavModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatToolbarModule,
    FormsModule,
    ReactiveFormsModule,
    NgxChartsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'courses', component: CourseComponent },  
      { path: 'players', component: PlayerComponent },
      { path: 'add-player', component: AddPlayerComponent },
      { path: 'golf-rounds', component: GolfRoundComponent },
      { path: 'add-golf-round', component: AddGolfRoundComponent },
      { path: 'sign-up', component: SignUpComponent },
      { path: 'login', component: LoginDialogComponent },
      { path: 'logout', component: NavMenuComponent },
      { path: 'add-partner', component: AddPartnerComponent },
      { path: 'about', component: AboutComponent },
      { path: 'partners', component: PartnerComponent }
    ])
  ],
  providers: [CourseService, PlayerService, GolfRoundService, ScoreService, UserService, PartnerService, MatSnackBar],
  bootstrap: [AppComponent]
})
export class AppModule { }
