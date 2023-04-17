import { Injectable } from "@angular/core";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { EstadosRepository } from "../domain/estados.repository";

@Injectable()
export class EstadosApp extends ApplicationCrud<AlertasEntity> {

  constructor(protected override repo: EstadosRepository) {
    super(repo);
  }
}
