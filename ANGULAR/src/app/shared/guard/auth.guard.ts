import { Injectable, Injector } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginApp } from '../../core/login/application/login.app';
import { ModelsService } from './model';

@Injectable()
export class AuthGuard implements CanActivate {

  public baseUrl: string;

  constructor(private router: Router, private modelService: ModelsService, private loginApp: LoginApp, protected injector: Injector) {
    this.baseUrl = injector.get('BASE_URL').trim();
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Observable<boolean>((observer: any) => {
      try {
        if ((this.modelService.user === undefined || this.modelService.user === null)) {
          this.loginApp.getUsuario().toPromise()
            .then((data) => {
              const user = data;
              if (user !== undefined && user !== null && user.id != undefined && user.id != null && user.id != "") {
                this.modelService.user = user;

                if (route.data !== undefined && route.data !== null && route.data["rol"] !== undefined && route.data["rol"] !== null) {
                  const roles = route.data["rol"] as Array<string>;
                  const rol = roles.find((item) => {
                    return item === this.modelService?.user?.rol;
                  })

                  if (this.modelService.user.rol !== rol)
                    window.location.href = this.baseUrl + "home";

                  observer.next(this.modelService.user.rol === rol);
                  observer.complete();
                }
                else {
                  observer.next(true);
                  observer.complete();
                }
              } else {
                window.location.href = this.baseUrl + "login";
                observer.next(false);
                observer.complete();
              }
            })
            .catch(() => {
              window.location.href = this.baseUrl + "login";
              observer.next(false);
              observer.complete();
            });
        }
        else {
          if (route.data !== undefined && route.data !== null && route.data["rol"] !== undefined && route.data["rol"] !== null) {
            const roles = route.data["rol"] as Array<string>;
            const rol = roles.find((item) => {
              return item === this.modelService?.user?.rol;
            })

            if (this.modelService.user.rol !== rol)
              window.location.href = this.baseUrl + "home";

            observer.next(this.modelService.user.rol === rol);
            observer.complete();
          }
          else {
            observer.next(true);
            observer.complete();
          }
        }
      }
      catch (e) {
        window.location.href = this.baseUrl + "login";
      }
    });
  }
}
