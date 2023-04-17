import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { RolesApp } from '../../../core/roles/application/roles.app';
import { RolesRepository } from '../../../core/roles/domain/roles.repository';
import { RolesRestRepository } from '../../../core/roles/infrastructure/roles.rest.repository';
import { AlertModule } from '../../../shared/alert/alert.module';
import { ButtonModule } from '../../../shared/button/button.module';
import { EditPanelModule } from '../../../shared/editpanel/editpanel.module';
import { GridModule } from '../../../shared/grid/grid.module';
import { ImageModule } from '../../../shared/image/image.module';
import { InputModule } from '../../../shared/input/input.module';
import { SearchCrudModule } from '../../../shared/search-crud/search-crud.module';
import { ChageUsersComponent } from './dialogs/cange-users.component';
import { ShowUsersComponent } from './dialogs/show-users.component';
import { RolesRoutingModule } from './roles-routing.module';
import { RolesComponent } from './roles.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RolesRoutingModule,
    MatIconModule,
    InputModule,
    AlertModule,
    ButtonModule,
    SearchCrudModule,
    EditPanelModule,
    ImageModule,
    GridModule
  ],
  declarations: [RolesComponent, ShowUsersComponent, ChageUsersComponent],
  providers: [RolesApp, { provide: RolesRepository, useClass: RolesRestRepository }],
  entryComponents: []
})
export class RolesModule { }
