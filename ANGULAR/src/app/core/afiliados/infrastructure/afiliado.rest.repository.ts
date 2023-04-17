import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliadoEntity } from "../domain/afiliado.entity";
import { AfiliadoRepository } from "../domain/afiliado.repository";

@Injectable()
export class AfiliadoRestRepository extends RepositoryCrud<AfiliadoEntity> implements AfiliadoRepository {
  public override Controller = "Afiliado";

  public ChangeEstado(model: AfiliadoEntity): Observable<ReturnDataG<AfiliadoEntity>> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/ChangeState`, model);
  }
}
