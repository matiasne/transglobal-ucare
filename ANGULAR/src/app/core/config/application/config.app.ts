import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { ConfigEntity } from "../domain/config.entity";
import { ConfigRepository } from "../domain/config.repository";

@Injectable()
export class ConfigApp extends ApplicationCrud<ConfigEntity> {

  constructor(protected override repo: ConfigRepository) {
    super(repo);
  }

  public SaveUsuarioActivosMaximos(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>> {
    return this.repo.SaveUsuarioActivosMaximos(model);
  }

  public SaveTiempoEnvioSMSSeconds(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>> {
    return this.repo.SaveTiempoEnvioSMSSeconds(model);
  }

}
