import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { MapaConfigEntity } from "./mapa-config.entity";

export abstract class MapaConfigRepository implements EntityRepositoryCrud<MapaConfigEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<MapaConfigEntity>>;
  public abstract Save(model: MapaConfigEntity): Observable<ReturnDataG<MapaConfigEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<MapaConfigEntity>>;
}
