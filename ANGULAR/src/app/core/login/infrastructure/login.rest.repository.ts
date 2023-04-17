import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { RepositoryBase } from "../../../shared/infrastructure/Repository";
import { LoginEntity } from "../domain/login.entity";
import { LoginRepository } from "../domain/login.repository";

@Injectable()
export class LoginRestRepository extends RepositoryBase implements LoginRepository {

  public getVersion(): Observable<string> {
    return this.get(`${this.baseUrl}api/v1/version`)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }

  public login(username: string, password: string): Observable<string> {
    return this.post(`${this.baseUrl}api/v1/LoginManager`, { username: username, password: password, plataforma: "web" })
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }

  public recuperar(username: string): Observable<boolean> {
    return this.post(`${this.baseUrl}api/v1/recuperar`, { username: username, signature :"web"})
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }

  public recuperarCambiar(id: string, codigo: string, password: string): Observable<boolean> {
    let body = { "id": id, "codigo": codigo,"password":password}
    return this.post(`${this.baseUrl}api/v1/RecuperarCambiar`, body)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }

  public getUsuario(): Observable<LoginEntity> {
    return this.get(`${this.baseUrl}api/v1/getuser`)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return new LoginEntity(item.data);
      }));
  }

  public renovarToken(): Observable<string> {
    return this.get(`${this.baseUrl}api/v1/RenewToken`)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }


  public logout(): Observable<boolean> {
    return this.get(`${this.baseUrl}api/v1/logout/web`)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }
}
