import { Injectable } from "@angular/core";
import { map, Observable } from "rxjs";
import { RepositoryBase } from "../../../shared/infrastructure/Repository";
import { HomeRepository } from "../domain/home.repository";

@Injectable()
export class HomeRestRepository extends RepositoryBase implements HomeRepository {


  public getCodigosPostales(): Observable<string[]> {
    return this.get(`${this.baseUrl}api/v1/home/GetCodigosPostales`)
      .pipe(map((item) => {
        if (item.isError ?? true) {
          throw new Error("error");
        }
        return item.data;
      }));
  }
}
