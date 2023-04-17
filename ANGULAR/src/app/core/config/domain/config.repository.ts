import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { ConfigEntity } from "./config.entity";

export abstract class ConfigRepository implements EntityRepositoryCrud<ConfigEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<ConfigEntity>>;
  public abstract Save(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<ConfigEntity>>;
  public abstract SaveUsuarioActivosMaximos(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>>;
  public abstract SaveTiempoEnvioSMSSeconds(model: ConfigEntity): Observable<ReturnDataG<ConfigEntity>>;
}
