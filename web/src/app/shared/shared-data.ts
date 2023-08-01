import { InjectionToken } from '@angular/core';

export interface SharedData {
  [key: string]: any;
}

export const SHARED_DATA = new InjectionToken<SharedData>('shared data between pages');
