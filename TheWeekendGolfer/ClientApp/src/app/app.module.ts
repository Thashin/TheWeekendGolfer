import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatNativeDateModule } from "@angular/material/core";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatDialogModule } from "@angular/material/dialog";
import { MatDividerModule } from "@angular/material/divider";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSelectModule } from "@angular/material/select";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatSnackBar, MatSnackBarModule } from "@angular/material/snack-bar";
import { MatSortModule } from "@angular/material/sort";
import { MatStepperModule } from "@angular/material/stepper";
import { MatTableModule } from "@angular/material/table";
import { MatToolbarModule } from "@angular/material/toolbar";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HttpClientXsrfModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { FlexLayoutModule } from "@angular/flex-layout";
import { NgxChartsModule } from "@swimlane/ngx-charts";
import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { HomeComponent } from "./home/home.component";
import { PlayerComponent } from "./player/player.component";
import { CourseComponent } from "./course/course.component";
import { CourseService } from "./services/course.service";
import { PlayerService } from "./services/player.service";
import { GolfRoundService } from "./services/golfRound.service";
import { GolfRoundComponent } from "./golfRound/golfRound.component";
import { AddPlayerComponent } from "./player/add-player.component";
import { ScoreService } from "./services/scores.service";
import { UserService } from "./services/user.service";
import { AddPartnerDialogComponent } from "./partner/add-partner-dialog.component";
import { PartnerService } from "./services/partner.service";
import { PartnerComponent } from "./partner/partner.component";
import { AboutComponent } from "./about/about.component";
import { LoginDialogComponent } from "./user/loginDialog.component";
import { SignUpDialogComponent } from "./user/signUpDialog.component";
import { AddGolfRoundDialogComponent } from "./golfRound/add-golfRound-Dialog.component";
import { ForecastDialogComponent } from "./forecast/forecast-Dialog.component";
import { ForecastService } from "./services/forecast.service";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PlayerComponent,
    PartnerComponent,
    CourseComponent,
    GolfRoundComponent,
    AddGolfRoundDialogComponent,
    AddPlayerComponent,
    SignUpDialogComponent,
    LoginDialogComponent,
    AddPartnerDialogComponent,
    ForecastDialogComponent,
    AboutComponent
  ],
  entryComponents: [
    LoginDialogComponent,
    ForecastDialogComponent,
    SignUpDialogComponent,
    AddGolfRoundDialogComponent,
    AddPartnerDialogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    HttpClientXsrfModule.withOptions({
      cookieName: "XSRF-TOKEN",
      headerName: "X-CSRF-TOKEN"
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
      { path: "", component: HomeComponent, pathMatch: "full" },
      { path: "courses", component: CourseComponent },
      { path: "players", component: PlayerComponent },
      { path: "add-player", component: AddPlayerComponent },
      { path: "golf-rounds", component: GolfRoundComponent },
      { path: "add-golf-round", component: AddGolfRoundDialogComponent },
      { path: "sign-up", component: SignUpDialogComponent },
      { path: "login", component: LoginDialogComponent },
      { path: "logout", component: NavMenuComponent },
      { path: "add-partner", component: AddPartnerDialogComponent },
      { path: "about", component: AboutComponent },
      { path: "partners", component: PartnerComponent },
      { path: "forecast", component: ForecastDialogComponent }
    ])
  ],
  providers: [
    CourseService,
    ForecastService,
    PlayerService,
    GolfRoundService,
    ScoreService,
    UserService,
    PartnerService,
    MatSnackBar
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
