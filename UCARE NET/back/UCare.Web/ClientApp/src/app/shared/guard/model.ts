import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { LoginEntity } from '../../core/login/domain/login.entity';

@Injectable()
export class ModelsService {

  private _user: LoginEntity | null = null;
  public get user(): LoginEntity | null { return this._user; }
  public set user(value: LoginEntity | null) { this._user = value; }

  public showHourglass = false;

  public eventLoginUser: EventEmitter<LoginEntity> = new EventEmitter<LoginEntity>();

  constructor(protected http: HttpClient, protected router: Router, injector: Injector) {

  }
}
