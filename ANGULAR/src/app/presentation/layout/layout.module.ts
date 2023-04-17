import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LoginApp } from '../../core/login/application/login.app';
import { LoginRepository } from '../../core/login/domain/login.repository';
import { LoginRestRepository } from '../../core/login/infrastructure/login.rest.repository';
import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    LayoutRoutingModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatTooltipModule,
  ],
  declarations: [LayoutComponent],
  providers: [LoginApp, { provide: LoginRepository, useClass: LoginRestRepository }],
  entryComponents: []
})
export class LayoutModule { }
