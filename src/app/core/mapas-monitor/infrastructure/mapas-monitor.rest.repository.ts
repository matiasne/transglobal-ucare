import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliadoEntity } from "../../afiliados/domain/afiliado.entity";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { MapaConfigEntity } from "../../mapa-config/domain/mapa-config.entity";
import { MapaMonitorRepository } from "../domain/mapas-monitor.repository";

@Injectable()
export class MapaMonitorRestRepository extends RepositoryCrud<AlertasEntity> implements MapaMonitorRepository {

  public override Controller = "MapaMonitor";

  public getMapConfig(): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/GetMapaConfig`);
  }

  public getAsignados(): Observable<ReturnDataG<Array<AlertasEntity>>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/GetAsignados`);
  }

  public getMoreInfo(alertaid: string): Observable<ReturnDataG<AfiliadoEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/GetMoreInfo/${alertaid}`);
  }
}
