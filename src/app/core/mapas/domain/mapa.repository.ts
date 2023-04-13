import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { MapaConfigEntity } from "../../mapa-config/domain/mapa-config.entity";

export abstract class MapaRepository implements EntityRepositoryCrud<AlertasEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<AlertasEntity>>;
  public abstract Save(model: AlertasEntity): Observable<ReturnDataG<AlertasEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<AlertasEntity>>;
  public abstract getMapConfig(): Observable<ReturnDataG<MapaConfigEntity>>;
  public abstract SaveConfig(model: MapaConfigEntity): Observable<ReturnDataG<MapaConfigEntity>>;
}
