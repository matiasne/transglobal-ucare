import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ServerErrorValidationRoutingModule } from './server-error-validation-routing.module';
import { ServerErrorValidationComponent } from './server-error-validation.component';

@NgModule({
  imports: [
    CommonModule,
    ServerErrorValidationRoutingModule
  ],
  declarations: [ServerErrorValidationComponent]
})
export class ServerErrorValidationModule { }
