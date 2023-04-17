import { Injectable } from "@angular/core";
import { ApplicationCrud } from "../../../shared/application/ApplicationBase";
import { MapaConfigEntity } from "../domain/mapa-config.entity";
import { MapaConfigRepository } from "../domain/mapa-config.repository";

@Injectable()
export class MapaConfigApp extends ApplicationCrud<MapaConfigEntity> {

  constructor(protected override repo: MapaConfigRepository) {
    super(repo);
  }
}
