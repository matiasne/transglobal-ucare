import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscriber } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ModelsService } from '../guard/model';

export class ReturnData {
  public data: object | string | boolean | number | any = "";
  public isError = true;
  public ticks = 0;
  public isSecurity = false;
}

export class ReturnDataG<T extends object> extends ReturnData {
  public override data: T | null = null;
}

@Injectable()
export class RestService {

  public baseUrl: string;
  public get URL(): string { return this.baseUrl }

  constructor(
    public router: Router,
    protected http: HttpClient,
    protected injector: Injector,
    protected modelService: ModelsService) {
    this.baseUrl = injector.get('BASE_URL').trim();
  }

  protected getToken(): string | null {
    let token = null;
    try {
      token = localStorage.getItem("sessionToken");
    } catch (e) {
    }
    return token;
  }

  protected forceReload() {
    if (environment.production) {
      const form = document.createElement('form');
      form.method = "POST";
      form.action = location.href;
      document.body.appendChild(form);
      form.submit();
    } else {
      window.location.reload();
    }
  }

  protected httpError(err: HttpErrorResponse): void {
    switch (err.status) {
      case 401: {
        //TODO se desactiva el siguiete codigo hasta verificar el problema
        //this.forceReload();
        break;
      }
      case 403: {
        window.location.href = this.baseUrl + "login";
        throw Error("No tiene permisos para acceder al metodo. Consulte con el administrador");
        break;
      }
    }
  }

  protected get(url: string, params: string | undefined = undefined): Observable<ReturnData> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'jwt': this.getToken() ?? ""
    });

    return new Observable<ReturnData>((observer) => {
      this.http.get(url + (params !== undefined ? params : ""), { headers: headers }).toPromise()
        .then((data) => {
          observer.next(data as ReturnData);
          observer.complete();
        })
        .catch(this.onError(observer));
    });
  }

  protected post(url: string, obj: any, contentType = 'application/json'): Observable<ReturnData> {
    const headers = new HttpHeaders({
      'Content-Type': contentType,
      'jwt': this.getToken() ?? ""
    });
    return new Observable<ReturnData>((observer) => {
      this.http.post(url, obj, { headers: headers }).toPromise()
        .then((data) => {
          observer.next(data as ReturnData);
          observer.complete();
        })
        .catch(this.onError(observer));
    });
  }

  protected postDonwload(url: string, obj: any, fileName: string = "file", contentType = 'application/json'): Observable<ReturnData> {
    const headers = new HttpHeaders({
      'Content-Type': contentType,
      'jwt': this.getToken() ?? ""
    });
    return new Observable<ReturnData>((observer) => {
      this.http.post(url, obj, { headers: headers, responseType: 'blob' as 'json' }).toPromise()
        .then((data) => {
          const blob = new Blob([data] as BlobPart[], { type: 'application/octet-stream' });
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.setAttribute('download', fileName);
          document.body.appendChild(link);
          link.click();
          setTimeout(() => { document.body.removeChild(link); }, 500);
          observer.next({ data: true, isError: false } as ReturnData);
          observer.complete();
        })
        .catch(this.onError(observer));
    });
  }

  protected put(url: string, obj: any, contentType = 'application/json'): Observable<ReturnData> {
    const headers = contentType === "multipart/form-data" ? new HttpHeaders({
      'jwt': this.getToken() ?? ""
    }) : new HttpHeaders({
      'Content-Type': contentType,
      'jwt': this.getToken() ?? ""
    });
    return new Observable<ReturnData>((observer) => {
      this.http.put(url, obj, { headers: headers }).toPromise()
        .then((data) => {
          observer.next(data as ReturnData);
          observer.complete();
        })
        .catch(this.onError(observer));
    });
  }

  protected delete(url: string, params: string | undefined = undefined): Observable<ReturnData> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'jwt': this.getToken() ?? ""
    });
    return new Observable<ReturnData>((observer) => {
      this.http.delete(url + (params !== undefined ? params : ""), { headers: headers }).toPromise()
        .then((data) => {
          observer.next({ data: data, isError: false } as ReturnData);
          observer.complete();
        })
        .catch(this.onError(observer));
    });
  }


  protected onError(observer: Subscriber<ReturnData>) {
    return (err: any) => {
      try {

        if (err instanceof HttpErrorResponse) {
          this.httpError(err);
        }
        observer.next({ data: err, isError: true } as ReturnData);
        observer.complete();
      }
      catch (errorCatch) {
        observer.next({ data: (errorCatch as any).message, isError: true } as ReturnData);
        observer.complete();
      }
    }
  }
}

@Injectable()
export class RestCrudService<T extends object> extends RestService {
  public Controller = "";
  public ApiVersion = "v1";

  public Find(page: Page): Observable<ReturnData> {
    return this.post(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/find`, page);
  }

  public Edit(id: any): Observable<ReturnData> {
    return this.get(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Edit/${id}`);
  }

  public Save(model: T): Observable<ReturnData> {
    return this.put(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Save`, model);
  }

  public Delete(id: any): Observable<ReturnData> {
    return this.delete(`${this.baseUrl}api/${this.ApiVersion}/${this.Controller}/Delete/${id}`);
  }
}

export class Page {
  constructor(data: Page | null = null) {
    if (data !== null) {
      this.pageIndex = data.pageIndex;
      this.pageCount = data.pageCount;
      this.pageSize = data.pageSize;
      this.recordCount = data.recordCount;
      this.filter = data.filter;
      this.order = data.order;
    }
  }

  public pageIndex = 1;
  public pageCount = 0;
  public pageSize = 20;
  public recordCount = 0;
  public filter: Array<PagingFilter> = new Array<PagingFilter>();
  public order: Array<PagingOrder> = new Array<PagingOrder>();
  public data: Array<object> | null = null;
}

export class PageGeneric<T extends object> extends Page {
  public override data: Array<T> = new Array<T>();
}

export class PagingFilter {
  public property = "";
  public condition: ">" | ">=" | "<" | "<=" | "=" | "!=" | "con" | "=|T" | "X" | "x" | "in" | "notin" | "inarray"= "con";
  public value: string | number | Date | object = "";
  public valueType = "";
}

export class PagingOrder {
  public property = "";
  public direction: "+" | "-" = "+";
}
