import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Constants } from './shared/Constants';
import { AuthGuard } from './shared/guard/auth.guard';

const routes: Routes = [
  { path: '', loadChildren: () => import('./presentation/layout/layout.module').then(m => m.LayoutModule), canActivate: [AuthGuard], data: { "className": "", "methodName": "" } },
  { path: Constants.LAYOUT_LOGIN, loadChildren: () => import('./presentation/login/login.module').then(m => m.LoginModule) },
  { path: Constants.LAYOUT_RECUPERAR, loadChildren: () => import('./presentation/recuperar/recuperar.module').then(m => m.RecuperarModule) },
  { path: Constants.LAYOUT_ERROR, loadChildren: () => import('./presentation/server-response/server-error/server-error.module').then(m => m.ServerErrorModule) },
  { path: Constants.LAYOUT_ACCESS_DENIED, loadChildren: () => import('./presentation/server-response/access-denied/access-denied.module').then(m => m.AccessDeniedModule) },
  { path: Constants.LAYOUT_NOT_FOUNDD, loadChildren: () => import('./presentation/server-response/not-found/not-found.module').then(m => m.NotFoundModule) },
  { path: '**', redirectTo: Constants.LAYOUT_NOT_FOUNDD }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
