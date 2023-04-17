import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';

import { ButtonModule } from '../button/button.module';
import { SearchComponent, MoAddButtonTemplateDirective } from './search.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatTooltipModule
  ],
  declarations: [SearchComponent, MoAddButtonTemplateDirective],
  exports: [SearchComponent, MoAddButtonTemplateDirective],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  entryComponents: [SearchComponent]
})
export class SearchModule { }
