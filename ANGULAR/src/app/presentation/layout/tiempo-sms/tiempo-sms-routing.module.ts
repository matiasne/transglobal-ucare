import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TiempoSMSComponent } from './tiempo-sms.component';

const routes: Routes = [
  {
    path: '',
    component: TiempoSMSComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TiempoSMSRoutingModule { }
