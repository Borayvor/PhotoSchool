import { DateTimeServerModel } from './models/date-time-server.model';
import { HttpService } from './services/http.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Angular 2 and .Net Core';

  constructor(private http: HttpService) {

  }

  ngOnInit(): void {
    // get antiforgerytoken
    this.http.get('datetime').subscribe(response => {
      const result = response.json() as DateTimeServerModel;
      const dt = new Date(result.GetDateTime).toLocaleString();
      console.log(dt);
    });
  }
}
