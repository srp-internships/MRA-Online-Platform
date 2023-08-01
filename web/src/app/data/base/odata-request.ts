import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';

export const ODATA_SERVICE = new InjectionToken<ODataService>('odata request');

export interface ODataService {
  executeQuery: (query?: string) => Observable<any>;
}
