import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MapaConfigApp } from '../../../core/mapa-config/application/mapa-config.app';
import { MapaConfigRepository } from '../../../core/mapa-config/domain/mapa-config.repository';
import { MapaConfigRestRepository } from '../../../core/mapa-config/infrastructure/mapa-config.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { MapaConfigRoutingModule } from './mapa-config-routing.module';
import { MapaConfigComponent } from './mapa-config.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MapaConfigRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
  ],
  declarations: [MapaConfigComponent],
  providers: [MapaConfigApp, { provide: MapaConfigRepository, useClass: MapaConfigRestRepository }],
  entryComponents: []
})
export class MapaConfigModule { }
