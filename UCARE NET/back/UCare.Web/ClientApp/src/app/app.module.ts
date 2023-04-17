import { CommonModule } from '@angular/common';
import { HttpClientJsonpModule, HttpClientModule } from '@angular/common/http';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginApp } from './core/login/application/login.app';
import { LoginRepository } from './core/login/domain/login.repository';
import { LoginRestRepository } from './core/login/infrastructure/login.rest.repository';
import { AuthGuard } from './shared/guard/auth.guard';
import { ModelsService } from './shared/guard/model';
import { RestService } from './shared/rest/rest.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    HttpClientModule,
    HttpClientJsonpModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    ModelsService,
    AuthGuard,
    LoginApp,
    RestService,
    { provide: LoginRepository, useClass: LoginRestRepository }],
  bootstrap: [AppComponent],
  schemas:
    [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }

