import { throwError as observableThrowError } from "rxjs";
import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Forecast } from "../models/forecast.model";

@Injectable()
export class ForecastService {
  theWeekendGolferUrl: string;

  constructor(private _http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.theWeekendGolferUrl = baseUrl;
  }

  getForecasts(courseId: string) {
    return this._http.get<Forecast[]>(
      this.theWeekendGolferUrl + "api/Forecast/Index?CourseId=" + courseId
    );
  }

  errorHandler(error: Response) {
    return observableThrowError(error);
  }
}
