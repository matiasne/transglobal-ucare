import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { EstadosApp } from '../../../core/estados/application/estados.app';
import { EstadosRepository } from '../../../core/estados/domain/estados.repository';
import { EstadosRestRepository } from '../../../core/estados/infrastructure/estados.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { ShowBitacoraComponent } from './dialogs/show-bitacora.component';
import { EstadosRoutingModule } from './estados-routing.module';
import { EstadosComponent } from './estados.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    EstadosRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GridModule,
    NgxSliderModule
  ],
  declarations: [EstadosComponent, ShowBitacoraComponent],
  providers: [EstadosApp, { provide: EstadosRepository, useClass: EstadosRestRepository }],
  entryComponents: []
})
export class EstadosModule { }
