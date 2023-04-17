import { Injectable } from "@angular/core";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { AlertasEntity } from "../../alertas/domain/alertas.entity";
import { EstadosRepository } from "../domain/estados.repository";

@Injectable()
export class EstadosRestRepository extends RepositoryCrud<AlertasEntity> implements EstadosRepository {
  public override Controller = "AlertasEstado";


}
