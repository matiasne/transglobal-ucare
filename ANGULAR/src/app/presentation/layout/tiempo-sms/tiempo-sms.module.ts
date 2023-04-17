import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { ConfigApp } from '../../../core/config/application/config.app';
import { ConfigRepository } from '../../../core/config/domain/config.repository';
import { ConfigRestRepository } from '../../../core/config/infrastructure/config.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { TiempoSMSRoutingModule } from './tiempo-sms-routing.module';
import { TiempoSMSComponent } from './tiempo-sms.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TiempoSMSRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
  ],
  declarations: [TiempoSMSComponent],
  providers: [ConfigApp, { provide: ConfigRepository, useClass: ConfigRestRepository }],
  entryComponents: []
})
export class TiempoSMSModule { }
