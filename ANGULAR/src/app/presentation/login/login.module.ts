import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { LoginApp } from '../../core/login/application/login.app';
import { LoginRepository } from "../../core/login/domain/login.repository";
import { LoginRestRepository } from '../../core/login/infrastructure/login.rest.repository';
import { ButtonModule } from '../../shared/button/button.module';
import { InputModule } from '../../shared/input/input.module';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    InputModule,
    ButtonModule,
    LoginRoutingModule,
  ],
  declarations: [LoginComponent],
  providers: [LoginApp, { provide: LoginRepository, useClass: LoginRestRepository }],
  entryComponents: []
})
export class LoginModule { }
