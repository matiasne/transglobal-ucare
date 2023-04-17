import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';

import { AgGridModule } from 'ag-grid-angular';
import { ButtonModule } from '../button/button.module';
import { GridModule } from '../grid/grid.module';
import { SearchModule } from '../search/search.module';
import { GQContentSearchTemplateDirective, SearchCrudComponent } from './search-crud.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    AgGridModule,
    SearchModule,
    GridModule,
    ButtonModule,
    MatPaginatorModule
  ],
  declarations: [SearchCrudComponent, GQContentSearchTemplateDirective],
  exports: [SearchCrudComponent, GQContentSearchTemplateDirective],
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  entryComponents: [SearchCrudComponent]
})
export class SearchCrudModule { }
