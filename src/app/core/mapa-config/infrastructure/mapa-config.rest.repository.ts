import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { RepositoryCrud } from "../../../shared/infrastructure/Repository";
import { ReturnDataG } from "../../../shared/rest/rest.service";
import { MapaConfigEntity } from "../domain/mapa-config.entity";
import { MapaConfigRepository } from "../domain/mapa-config.repository";

@Injectable()
export class MapaConfigRestRepository extends RepositoryCrud<MapaConfigEntity> implements MapaConfigRepository {
  public override Controller = "MapaConfig";

  public override Edit(id: any): Observable<ReturnDataG<MapaConfigEntity>> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}`);
  }
}
