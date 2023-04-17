import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { LoginApp } from '../../core/login/application/login.app';
import { Constants } from '../../shared/Constants';
import { ModelsService } from '../../shared/guard/model';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {
  public openMenu: boolean = false;
  public menuState: any = {};
  public baseUrl: string;

  public LAYOUT_USUARIOS = Constants.LAYOUT_USUARIOS;
  public LAYOUT_ROLES = Constants.LAYOUT_ROLES;
  public LAYOUT_TEMPO_SMS = Constants.LAYOUT_TEMPO_SMS;
  public LAYOUT_ESTADOS = Constants.LAYOUT_ESTADOS;
  public LAYOUT_MAPA = Constants.LAYOUT_MAPA;
  public LAYOUT_MAPA_CONFIG = Constants.LAYOUT_MAPA_CONFIG;
  public LAYOUT_AFILIADO = Constants.LAYOUT_AFILIADO;
  public LAYOUT_MONITOREO = Constants.LAYOUT_MONITOREO;
  public LAYOUT_COMUNICACION = Constants.LAYOUT_COMUNICACION;
  public timer: any;

  constructor(public modelsService: ModelsService,
    private router: Router,
    protected injector: Injector,
    private loginApp: LoginApp,) {

    this.baseUrl = injector.get('BASE_URL').trim();
  }
  ngOnDestroy(): void {
    try {
      clearInterval(this.timer);
    }
    catch (error) { }
  }

  ngOnInit(): void {
    setTimeout(async () => {
      await lastValueFrom(this.loginApp.renovarToken());
    }, 10000);

    this.timer = setInterval(async () => {
      await lastValueFrom(this.loginApp.renovarToken());
    }, 60 * 60 * 1000);
  }

  public onOpenCloseMenu() {
    this.openMenu = !this.openMenu;
  }

  public onOpenMenu(menu: string) {
    this.menuState[menu] = !(this.menuState[menu] === undefined || this.menuState[menu] === null ? false : this.menuState[menu]);
  }

  public isCloseMenu(menu: string) {
    return !(this.menuState[menu] === undefined || this.menuState[menu] === null ? false : this.menuState[menu])
  }

  public onOpenRoute(url: string, close = false) {
    if (close) {
      this.openMenu = false;
    }
    this.router.navigate([url + "/find"]);
  }

  public async onSalir() {
    await lastValueFrom(this.loginApp.logout());
    window.location.href = this.baseUrl + "login";
  }

}
