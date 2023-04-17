import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnData, ReturnDataG } from "../../../shared/rest/rest.service";
import { RolesEntity } from "../domain/roles.entity";
import { RolesRepository } from "../domain/roles.repository";

@Injectable()
export class RolesApp extends ApplicationCrud<RolesEntity> {

  constructor(protected override repo: RolesRepository) {
    super(repo);
  }

  public GetUser(id: string): Observable<ReturnDataG<Array<RolesEntity>>> {
    return this.repo.GetUser(id);
  }

  public GetReplace(id: string): Observable<ReturnDataG<Array<RolesEntity>>> {
    return this.repo.GetReplace(id);
  }

  public GetReplaceTo(id: string, idTo: string): Observable<ReturnData> {
    return this.repo.GetReplaceTo(id, idTo);
  }

  public GetAllUserManager(): Observable<ReturnDataG<RolesEntity[]>> {
    return this.repo.GetAllUserManager();
  }

  public GetCodigosPostales(): Observable<ReturnDataG<string[]>> {
    return this.repo.GetCodigosPostales();
  }

  
}
