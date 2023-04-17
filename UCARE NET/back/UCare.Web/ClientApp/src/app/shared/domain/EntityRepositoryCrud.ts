import { Observable } from "rxjs";
import { Page, ReturnDataG } from "../rest/rest.service";

export interface EntityRepositoryCrud<TEntity extends object> {
  Find(page: Page): Observable<ReturnDataG<Page>>;

  Edit(id: any): Observable<ReturnDataG<TEntity>>;

  Save(model: TEntity): Observable<ReturnDataG<TEntity>>;

  Delete(id: any): Observable<ReturnDataG<TEntity>>;
}
