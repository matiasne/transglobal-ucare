import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { ConfigEntity } from "../domain/config.entity";
import { ConfigRepository } from "../domain/config.repository";

@Injectable()
export class ConfigRestRepository extends RepositoryCrud<ConfigEntity> implements ConfigRepository {
  public override Controller = "Config";

  public override Edit(id: any): Observable<ReturnDataG<ConfigEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`);
  }

  public SaveUsuarioActivosMaximos(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/SaveUsuarioActivosMaximos`, model);
  }

  public SaveTiempoEnvioSMSSeconds(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/SaveTiempoEnvioSMSSeconds`, model);
  }
  
}
