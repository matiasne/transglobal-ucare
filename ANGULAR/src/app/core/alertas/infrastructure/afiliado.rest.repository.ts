import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG, ReturnData } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "../domain/alertas.entity";
import { AlertasRepository } from "../domain/alertas.repository";

@Injectable()
export class AlertaRestRepository extends RepositoryCrud<AlertasEntity> implements AlertasRepository {
  public override Controller = "Users";


  public GetUser(id: any): Observable<ReturnDataG<AlertasEntity[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Users/${id}`);
  }
  public GetReplace(id: any): Observable<ReturnDataG<AlertasEntity[]>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Replace/${id}`);
  }
  public GetReplaceTo(id: any, idTo: string): Observable<ReturnData> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/ReplaceTo/${id}/${idTo}`);
  }


}
