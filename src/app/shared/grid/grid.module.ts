import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AgGridModule } from 'ag-grid-angular';
import { AgActionButtonsComponent, FilterButtons } from './action-buttons.component';
import { GridComponent } from './grid.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ImageFormatterComponent } from ".//ImageFormatterComponent";

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatInputModule,
    MatFormFieldModule,
    AgGridModule,
  ],
  declarations: [GridComponent, AgActionButtonsComponent, FilterButtons, ImageFormatterComponent],
  exports: [GridComponent, AgActionButtonsComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA],
  entryComponents: [GridComponent, AgActionButtonsComponent],
})
export class GridModule { }
