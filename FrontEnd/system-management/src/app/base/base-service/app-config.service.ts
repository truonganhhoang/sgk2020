import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IAppConfig } from '../base-model/app-config.model';

@Injectable()
export class ConfigService {

  static settings: IAppConfig;

  constructor(private http: HttpClient) {
  }

}
