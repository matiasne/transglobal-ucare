import { Observable } from "rxjs";
import { Page, RestService, ReturnDataG } from "../rest/rest.service";

export abstract class RepositoryBase extends RestService {
  public Controller = "";
  public ApiVersion = "v1";
}

export abstract class RepositoryCrud<TEntity extends object> extends RepositoryBase {

  public Find(page: Page): Observable<ReturnDataG<Page>> {
    return this.post(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`, page);
  }

  public Edit(id: any): Observable<ReturnDataG<TEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/${id}`);
  }

  public Save(model: TEntity): Observable<ReturnDataG<TEntity>> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`, model);
  }

  public Delete(id: any): Observable<ReturnDataG<TEntity>> {
    return this.delete(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/${id}`);
  }
}
