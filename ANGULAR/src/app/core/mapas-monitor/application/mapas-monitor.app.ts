import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliadoEntity } from "../../afiliados/domain/afiliado.entity";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { MapaConfigEntity } from "../../mapa-config/domain/mapa-config.entity";
import { MapaMonitorRepository } from "../domain/mapas-monitor.repository";

@Injectable()
export class MapaMonitorApp extends ApplicationCrud<AlertasEntity> {

  constructor(protected override repo: MapaMonitorRepository) {
    super(repo);
  }

  public getMapConfig(): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.repo.getMapConfig();
  }

  public getAsignados(): Observable<ReturnDataG<Array<AlertasEntity>>> {
    return this.repo.getAsignados();
  }
  public getMoreInfo(alertaid: string): Observable<ReturnDataG<AfiliadoEntity>> {
    return this.repo.getMoreInfo(alertaid);
  }
}
