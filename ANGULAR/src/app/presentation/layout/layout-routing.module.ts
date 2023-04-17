import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Constants } from '../../shared/Constants';
import { AuthGuard } from '../../shared/guard/auth.guard';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', redirectTo: Constants.LAYOUT_HOME },
      { path: Constants.LAYOUT_HOME, loadChildren: () => import('./home/home.module').then(m => m.HomeModule), canActivate: [AuthGuard], data: {} },
      { path: Constants.LAYOUT_USUARIOS + '/:id', loadChildren: () => import('./usuarios/usuarios.module').then(m => m.UsuariosModule), canActivate: [AuthGuard], data: { "rol": ["P"] } },
      { path: Constants.LAYOUT_TEMPO_SMS + '/:id', loadChildren: () => import('./tiempo-sms/tiempo-sms.module').then(m => m.TiempoSMSModule), canActivate: [AuthGuard], data: { "rol": ["G"] } },
      { path: Constants.LAYOUT_ROLES + '/:id', loadChildren: () => import('./roles/roles.module').then(m => m.RolesModule), canActivate: [AuthGuard], data: { "rol": ["P", "G", "A"] } },
      { path: Constants.LAYOUT_ESTADOS + '/:id', loadChildren: () => import('./estados/estados.module').then(m => m.EstadosModule), canActivate: [AuthGuard], data: { "rol": ["A"] } },
      { path: Constants.LAYOUT_MAPA + '/:id', loadChildren: () => import('./mapa/mapa.module').then(m => m.MapaModule), canActivate: [AuthGuard], data: { "rol": ["A"] } },
      { path: Constants.LAYOUT_MAPA_CONFIG + '/:id', loadChildren: () => import('./mapa-config/mapa-config.module').then(m => m.MapaConfigModule), canActivate: [AuthGuard], data: { "rol": ["A"] } },
      { path: Constants.LAYOUT_AFILIADO + '/:id', loadChildren: () => import('./validar/validar.module').then(m => m.ValidarModule), canActivate: [AuthGuard], data: { "rol": ["V"] } },
      { path: Constants.LAYOUT_MONITOREO + '/:id', loadChildren: () => import('./mapa-monitor/mapa-monitor.module').then(m => m.MapaMonitorModule), canActivate: [AuthGuard], data: { "rol": ["M"] } },
      { path: Constants.LAYOUT_COMUNICACION + '/:id', loadChildren: () => import('./comunicaciones/comunicacion.module').then(m => m.ComunicacionModule), canActivate: [AuthGuard], data: { "rol": ["A"] } },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
