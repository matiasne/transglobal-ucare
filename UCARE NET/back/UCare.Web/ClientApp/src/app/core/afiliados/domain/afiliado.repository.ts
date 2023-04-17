import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliadoEntity } from "./afiliado.entity";

export abstract class AfiliadoRepository implements EntityRepositoryCrud<AfiliadoEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<AfiliadoEntity>>;
  public abstract Save(model: AfiliadoEntity): Observable<ReturnDataG<AfiliadoEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<AfiliadoEntity>>;

  public abstract ChangeEstado(model: AfiliadoEntity): Observable<ReturnDataG<AfiliadoEntity>>;
}
