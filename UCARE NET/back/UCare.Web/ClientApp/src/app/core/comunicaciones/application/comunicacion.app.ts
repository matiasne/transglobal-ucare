import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { Page, ReturnDataG } from "../../../shared/rest/rest.service";
import { AfiliacionEntity } from "../../afiliados/domain/afiliado.entity";
import { ComunicacionEntity } from "../domain/comunicacion.entity";
import { ComunicacionRepository } from "../domain/comunicacion.repository";

@Injectable()
export class ComunicacionApp extends ApplicationCrud<ComunicacionEntity> {

  constructor(protected override repo: ComunicacionRepository) {
    super(repo);
  }

  public FindAfiliados(page: Page): Observable<ReturnDataG<Array<AfiliacionEntity>>> {
    return this.repo.FindAfiliados(page);
  }
}
