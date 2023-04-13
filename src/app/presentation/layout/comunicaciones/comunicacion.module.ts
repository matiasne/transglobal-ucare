import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ComunicacionApp } from '../../../core/comunicaciones/application/comunicacion.app';
import { ComunicacionRepository } from '../../../core/comunicaciones/domain/comunicacion.repository';
import { ComunicacionRestRepository } from '../../../core/comunicaciones/infrastructure/comunicacion.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { ComunicacionRoutingModule } from './comunicacion-routing.module';
import { ComunicacionComponent } from './comunicacion.component';
import { EdicionComunicacionComponent } from './dialogs/edicion-comunicacion.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ComunicacionRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GridModule,
    NgxSliderModule,
    MatSlideToggleModule,
  ],
  declarations: [ComunicacionComponent, EdicionComunicacionComponent],
  providers: [ComunicacionApp, { provide: ComunicacionRepository, useClass: ComunicacionRestRepository }],
  entryComponents: []
})
export class ComunicacionModule { }
