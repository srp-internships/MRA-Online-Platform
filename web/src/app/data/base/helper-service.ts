import { InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { Documentation } from 'src/app/domain/documentation';

export const HELPER_SERVICE = new InjectionToken<HelperService>('helper service');

export interface HelperService {
  help: () => Observable<Documentation>;
}
