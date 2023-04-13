import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliacionEntity } from "../../afiliados/domain/afiliado.entity";
import { ComunicacionEntity } from "../domain/comunicacion.entity";
import { ComunicacionRepository } from "../domain/comunicacion.repository";

@Injectable()
export class ComunicacionRestRepository extends RepositoryCrud<ComunicacionEntity> implements ComunicacionRepository {
  public override Controller = "Comunicacion";


  public FindAfiliados(page: Page): Observable<ReturnDataG<AfiliacionEntity[]>> {
    return this.post(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/FindAfiliados/`, page);
  }

}
