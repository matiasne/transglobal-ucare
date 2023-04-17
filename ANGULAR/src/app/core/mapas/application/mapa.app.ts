import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { MapaConfigEntity } from "../../mapa-config/domain/mapa-config.entity";
import { MapaRepository } from "../domain/mapa.repository";

@Injectable()
export class MapaApp extends ApplicationCrud<AlertasEntity> {

  constructor(protected override repo: MapaRepository) {
    super(repo);
  }

  public getMapConfig(): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.repo.getMapConfig();
  }
  public SaveConfig(model: MapaConfigEntity): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.repo.SaveConfig(model);
  }
}
