import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html'
})
export class PlayerComponent {
  public forecasts: WeatherForecast[];
  public showAddModal: Boolean = false;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }

  public toggleAddModal() {
    this.showAddModal = !this.showAddModal;
  }

  public addPlayer() {

  }
}

interface WeatherForecast {
  dateFormatted: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
