import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../../../shared/domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliacionEntity } from "../../afiliados/domain/afiliado.entity";
import { ComunicacionEntity } from "./comunicacion.entity";

export abstract class ComunicacionRepository implements EntityRepositoryCrud<ComunicacionEntity> {
  public abstract Find(page: Page): Observable<ReturnDataG<Page>>;
  public abstract Edit(id: any): Observable<ReturnDataG<ComunicacionEntity>>;
  public abstract Save(model: ComunicacionEntity): Observable<ReturnDataG<ComunicacionEntity>>;
  public abstract Delete(id: any): Observable<ReturnDataG<ComunicacionEntity>>;
  public abstract FindAfiliados(page: Page): Observable<ReturnDataG<Array<AfiliacionEntity>>>;

}
