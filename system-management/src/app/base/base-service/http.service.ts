import { Injectable } from "@angular/core";
import {
  HttpClient,
  HttpHeaders,
  HttpParams,
  HttpHandler,
  HttpEvent
} from "@angular/common/http";
import { Observable } from "rxjs";
export interface IRequestOptions {
  headers?: HttpHeaders;
  observe?: "body";
  params?: HttpParams | {};
  reportProgress?: boolean;
  responseType?: "json";
  withCredentials?: boolean;
  body?: any;
}

@Injectable({
  providedIn: "root"
})
export class HttpService {
  constructor(private http: HttpClient) {}

  public get<T>(endpoint: string, options?: IRequestOptions): Observable<T> {
    return this.http.get<T>(endpoint, options);
  }
  public post<T>(
    endpoint: string,
    params: Object,
    options?: IRequestOptions
  ): Observable<T> {
    return this.http.post<T>(endpoint, params, options);
  }
  public put<T>(
    endpoint: string,
    params: Object,
    options?: IRequestOptions
  ): Observable<T> {
    return this.http.put<T>(endpoint, params, options);
  }
  public delete<T>(endpoint: string, options?: IRequestOptions): Observable<T> {
    return this.http.delete<T>(endpoint, options);
  }
}
