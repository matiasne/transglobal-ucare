import { Observable } from "rxjs";
import { EntityRepositoryCrud } from "../domain/EntityRepositoryCrud";
import { Page, ReturnDataG } from "../rest/rest.service";

export abstract class ApplicationBase {

}

export abstract class ApplicationCrud<TEntity extends object> extends ApplicationBase {

  constructor(protected repo: EntityRepositoryCrud<TEntity>) {
    super();
  }

  public Find(page: Page): Observable<ReturnDataG<Page>> {
    return this.repo.Find(page);
  }

  public Edit(id: any): Observable<ReturnDataG<TEntity>> {
    return this.repo.Edit(id);
  }

  public Save(model: TEntity): Observable<ReturnDataG<TEntity>> {
    return this.repo.Save(model);
  }

  public Delete(id: any): Observable<ReturnDataG<TEntity>> {
    return this.repo.Delete(id);
  }
}
