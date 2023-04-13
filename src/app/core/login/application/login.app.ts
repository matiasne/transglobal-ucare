import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { ApplicationBase } from "../../../shared/application/ApplicationBase";
import { LoginEntity } from "../domain/login.entity";
import { LoginRepository } from "../domain/login.repository";

@Injectable()
export class LoginApp extends ApplicationBase {

  constructor(private loginRepository: LoginRepository) {
    super();
  }
  public getVersion(): Observable<string> {
    return this.loginRepository.getVersion();
  }

  public login(username: string, password: string): Observable<string> {
    return this.loginRepository.login(username, password).pipe(map((item) => {
      localStorage.setItem("sessionToken", item);
      return item;
    }));
  }

  public recuperar(username: string): Observable<boolean> {
    return this.loginRepository.recuperar(username).pipe(map((item) => {
      return item;
    }));
  }

  public recuperarCambiar(id: string, codigo: string, password: string): Observable<boolean> {
    return this.loginRepository.recuperarCambiar(id, codigo, password).pipe(map((item) => {
      return item;
    }));
  }

  public getUsuario(): Observable<LoginEntity> {
    return this.loginRepository.getUsuario();
  }

  public renovarToken(): Observable<string> {
    return this.loginRepository.renovarToken().pipe(map((item) => {
      localStorage.setItem("sessionToken", item);
      return item;
    }));
  }

  public logout(): Observable<boolean> {
    return this.loginRepository.logout().pipe(map((item) => {
      localStorage.removeItem("sessionToken");
      return item;
    }));
  }
}
