import { Observable } from "rxjs";

export abstract class HomeRepository {
  abstract getCodigosPostales(): Observable<string[]>;
}
