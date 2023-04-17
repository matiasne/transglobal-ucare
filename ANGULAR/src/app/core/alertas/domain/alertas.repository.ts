import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnData, ReturnDataG } from "../../../shared/rest/rest.service";
import { AlertasEntity } from "./alertas.entity";

export abstract class AlertasRepository implements EntityRepositoryCrud<AlertasEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<AlertasEntity>>;
  public abstract Save(model: AlertasEntity): Observable<ReturnDataG<AlertasEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<AlertasEntity>>;
  public abstract GetUser(id: any): Observable<ReturnDataG<Array<AlertasEntity>>>;
  public abstract GetReplace(id: any): Observable<ReturnDataG<Array<AlertasEntity>>>;
  public abstract GetReplaceTo(id: any, idTo: string): Observable<ReturnData>;

}
