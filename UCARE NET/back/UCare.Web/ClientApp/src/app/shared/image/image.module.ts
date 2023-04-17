import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from '../button/button.module';
import { ImageComponent } from './image.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
  ],
  declarations: [ImageComponent],
  exports: [ImageComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  entryComponents: [ImageComponent]
})
export class ImageModule { }
