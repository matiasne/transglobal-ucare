import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapaMonitorComponent } from './mapa-monitor.component';

const routes: Routes = [
  {
    path: '',
    component: MapaMonitorComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MapaMonitorRoutingModule { }
