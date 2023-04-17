import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnData, ReturnDataG } from "../../../shared/rest/rest.service";
import { RolesEntity } from "./roles.entity";

export abstract class RolesRepository implements EntityRepositoryCrud<RolesEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<RolesEntity>>;
  public abstract Save(model: RolesEntity): Observable<ReturnDataG<RolesEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<RolesEntity>>;
  public abstract GetUser(id: any): Observable<ReturnDataG<Array<RolesEntity>>>;
  public abstract GetReplace(id: any): Observable<ReturnDataG<Array<RolesEntity>>>;
  public abstract GetReplaceTo(id: any, idTo: string): Observable<ReturnData>;
  public abstract GetAllUserManager(): Observable<ReturnDataG<RolesEntity[]>>;
  public abstract GetCodigosPostales(): Observable<ReturnDataG<string[]>>

}
