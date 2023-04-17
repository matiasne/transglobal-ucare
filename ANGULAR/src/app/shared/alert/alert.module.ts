import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';

import { AlertComponent } from './alert.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
  ],
  providers: [],
  declarations: [AlertComponent],
  entryComponents: [AlertComponent],
  exports: [AlertComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AlertModule { }
