import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliadoEntity } from "../domain/afiliado.entity";
import { AfiliadoRepository } from "../domain/afiliado.repository";

@Injectable()
export class AfiliadoApp extends ApplicationCrud<AfiliadoEntity> {

  constructor(protected override repo: AfiliadoRepository) {
    super(repo);
  }

  public ChangeEstado(model: AfiliadoEntity): Observable<ReturnDataG<AfiliadoEntity>> {
    return this.repo.ChangeEstado(model);
  }
  
}
