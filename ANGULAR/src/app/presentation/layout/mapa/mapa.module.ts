import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GoogleMapsModule } from '@angular/google-maps';
import { MatIconModule } from '@angular/material/icon';
import { MapaApp } from '../../../core/mapas/application/mapa.app';
import { MapaRepository } from '../../../core/mapas/domain/mapa.repository';
import { MapaRestRepository } from '../../../core/mapas/infrastructure/mapa.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { MapaRoutingModule } from './mapa-routing.module';
import { MapaComponent } from './mapa.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientJsonpModule,
    ReactiveFormsModule,
    MapaRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GoogleMapsModule,
    GridModule,
    NgxSliderModule
  ],
  declarations: [MapaComponent],
  providers: [MapaApp, { provide: MapaRepository, useClass: MapaRestRepository }],
  entryComponents: []
})
export class MapaModule { }
