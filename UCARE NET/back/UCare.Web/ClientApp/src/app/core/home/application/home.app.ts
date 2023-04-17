import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ApplicationBase } from "../../../shared/application/ApplicationBase";
import { HomeRepository } from "../domain/home.repository";

@Injectable()
export class HomeApp extends ApplicationBase {

  constructor(private homeRepository: HomeRepository) {
    super();
  }
  public getCodigosPostales(): Observable<string[]> {
    return this.homeRepository.getCodigosPostales();
  }
}
