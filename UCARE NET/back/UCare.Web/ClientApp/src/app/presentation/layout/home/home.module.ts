import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { HomeApp } from '../../../core/home/application/home.app';
import { HomeRepository } from '../../../core/home/domain/home.repository';
import { HomeRestRepository } from '../../../core/home/infrastructure/home.rest.repository';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatSlideToggleModule,
    MatIconModule,
    MatTooltipModule,
    HomeRoutingModule
  ],
  declarations: [HomeComponent],
  providers: [HomeApp, { provide: HomeRepository, useClass: HomeRestRepository }],
  entryComponents: []
})
export class HomeModule { }
