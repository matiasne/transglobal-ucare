import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapaConfigComponent } from './mapa-config.component';

const routes: Routes = [
  {
    path: '',
    component: MapaConfigComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MapaConfigRoutingModule { }
