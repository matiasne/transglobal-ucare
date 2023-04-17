import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG, ReturnData } from "../../../shared/rest/rest.service";
import { RolesEntity } from "../domain/roles.entity";
import { RolesRepository } from "../domain/roles.repository";

@Injectable()
export class RolesRestRepository extends RepositoryCrud<RolesEntity> implements RolesRepository {

  public override Controller = "Users";


  public GetUser(id: any): Observable<ReturnDataG<RolesEntity[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Users/${id}`);
  }
  public GetReplace(id: any): Observable<ReturnDataG<RolesEntity[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Replace/${id}`);
  }
  public GetReplaceTo(id: any, idTo: string): Observable<ReturnData> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/ReplaceTo/${id}/${idTo}`);
  }
  public GetAllUserManager(): Observable<ReturnDataG<RolesEntity[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/GetAllUserManager`);
  }
  public GetCodigosPostales(): Observable<ReturnDataG<string[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/CodigosPostales`);
  }
}
