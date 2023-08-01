import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Documentation } from 'src/app/domain/documentation';
import { HelperService } from './helper-service';
import { ODataService } from './odata-request';

export abstract class BaseService implements ODataService, HelperService {
  protected abstract segment: string;
  constructor(protected _apiUrl: string, protected _http: HttpClient) {}

  protected getPath(hasSlesh: boolean = true): string {
    return this._apiUrl + this.segment + (hasSlesh ? '/' : '');
  }

  executeQuery(query: string = ''): Observable<any> {
    return this._http.get(`${this._apiUrl}odata/${this.segment}?${query}`);
  }

  help(): Observable<Documentation> {
    return this._http.get<Documentation>(`${this.getPath()}help`);
  }
}
