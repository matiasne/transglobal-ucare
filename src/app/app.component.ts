import { Component } from '@angular/core';
import * as moment from 'moment';
import { ModelsService } from './shared/guard/model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public modelsService: ModelsService) {
    moment.locale("es");
}
}
