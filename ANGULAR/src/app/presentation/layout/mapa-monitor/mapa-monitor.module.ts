import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GoogleMapsModule } from '@angular/google-maps';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MapaMonitorApp } from '../../../core/mapas-monitor/application/mapas-monitor.app';
import { MapaMonitorRepository } from '../../../core/mapas-monitor/domain/mapas-monitor.repository';
import { MapaMonitorRestRepository } from '../../../core/mapas-monitor/infrastructure/mapas-monitor.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { AlertaInfoComponent } from './components/alerta-info.component';
import { AlarmaActivarAlertaComponent } from './dialogs/alarma-activar-alerta.component';
import { ChangeEstadoAlertaComponent } from './dialogs/change-estado-alerta.component';
import { CountDownPauseAlertaComponent } from './dialogs/count-down-pause.component';
import { MonitorPausaComponent } from './dialogs/monitor-pausa.component';
import { WriteAlertaBitacoraComponent } from './dialogs/write-bitacora-alerta.component';
import { MapaMonitorRoutingModule } from './mapa-monitor-routing.module';
import { MapaMonitorComponent } from './mapa-monitor.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientJsonpModule,
    ReactiveFormsModule,
    MapaMonitorRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GoogleMapsModule,
    GridModule,
    NgxSliderModule,
    MatSlideToggleModule,
  ],
  declarations: [MapaMonitorComponent, ChangeEstadoAlertaComponent, WriteAlertaBitacoraComponent, AlarmaActivarAlertaComponent, CountDownPauseAlertaComponent, MonitorPausaComponent, AlertaInfoComponent],
  providers: [MapaMonitorApp, { provide: MapaMonitorRepository, useClass: MapaMonitorRestRepository }],
  entryComponents: []
})
export class MapaMonitorModule { }
