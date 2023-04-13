import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { MapaConfigEntity } from "../../mapa-config/domain/mapa-config.entity";
import { MapaRepository } from "../domain/mapa.repository";

@Injectable()
export class MapaRestRepository extends RepositoryCrud<AlertasEntity> implements MapaRepository {
  public override Controller = "Mapa";

  public getMapConfig(): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`);
  }

  public SaveConfig(model: MapaConfigEntity): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`, model);
  }
}
