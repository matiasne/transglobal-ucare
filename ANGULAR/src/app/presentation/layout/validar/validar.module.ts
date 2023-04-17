import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { AfiliadoApp } from '../../../core/afiliados/application/afiliado.app';
import { AfiliadoRepository } from '../../../core/afiliados/domain/afiliado.repository';
import { AfiliadoRestRepository } from '../../../core/afiliados/infrastructure/afiliado.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { ChangeEstadoComponent } from './dialogs/change-estado.component';
import { ValidarRoutingModule } from './validar-routing.module';
import { ValidarComponent } from './validar.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ValidarRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GridModule
  ],
  declarations: [ValidarComponent, ChangeEstadoComponent],
  providers: [AfiliadoApp, { provide: AfiliadoRepository, useClass: AfiliadoRestRepository }],
  entryComponents: []
})
export class ValidarModule { }
