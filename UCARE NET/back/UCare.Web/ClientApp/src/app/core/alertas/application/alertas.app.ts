import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnData, ReturnDataG } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "../domain/alertas.entity";
import { AlertasRepository } from "../domain/alertas.repository";

@Injectable()
export class AlertaApp extends ApplicationCrud<AlertasEntity> {

  constructor(protected override repo: AlertasRepository) {
    super(repo);
  }

  public GetUser(id: string): Observable<ReturnDataG<Array<AlertasEntity>>> {
    return this.repo.GetUser(id);
  }

  public GetReplace(id: string): Observable<ReturnDataG<Array<AlertasEntity>>> {
    return this.repo.GetReplace(id);
  }

  public GetReplaceTo(id: string, idTo: string): Observable<ReturnData> {
    return this.repo.GetReplaceTo(id, idTo);
  }

}
