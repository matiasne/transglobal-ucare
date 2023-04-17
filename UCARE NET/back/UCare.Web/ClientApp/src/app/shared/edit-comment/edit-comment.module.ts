import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { QuillModule } from 'ngx-quill';
import { MOEditCommentComponent, SafePipe } from './edit-comment.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    QuillModule,
    MatInputModule,
    MatIconModule,
  ],
  providers: [SafePipe],
  declarations: [MOEditCommentComponent, SafePipe],
  entryComponents: [MOEditCommentComponent],
  exports: [MOEditCommentComponent, SafePipe],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class MOEditCommentModule { }
