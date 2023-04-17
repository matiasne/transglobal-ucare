import { Component, OnInit } from '@angular/core';
import { HomeApp } from '../../../core/home/application/home.app';
import { ModelsService } from '../../../shared/guard/model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public codigosPostales: string[] = [];

  constructor(public modelsService: ModelsService, public app: HomeApp) {
    app.getCodigosPostales().subscribe((result) => {
      this.codigosPostales = result;
    });
  }

  ngOnInit(): void {
  }

}
