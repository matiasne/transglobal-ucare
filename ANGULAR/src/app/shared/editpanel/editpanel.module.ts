import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditPanelComponent } from './editpanel.component';
import { ButtonModule } from '../button/button.module';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    ButtonModule,
  ],
  declarations: [EditPanelComponent],
  exports: [EditPanelComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  entryComponents: [EditPanelComponent]
})
export class EditPanelModule { }
