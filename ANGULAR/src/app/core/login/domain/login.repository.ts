import { Observable } from "rxjs";
import { LoginEntity } from "./login.entity";

export abstract class LoginRepository {
  abstract login(username: string, password: string): Observable<string>;
  abstract renovarToken(): Observable<string>;
  abstract getUsuario(): Observable<LoginEntity>;
  abstract logout(): Observable<boolean>;
  abstract getVersion(): Observable<string>;
  
  abstract recuperar(username: string): Observable<boolean>;
  abstract recuperarCambiar(id: string, codigo: string, password: string): Observable<boolean>;
}
