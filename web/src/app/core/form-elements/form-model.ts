import { InjectionToken } from '@angular/core';
import { BaseFormElementService } from './form-element.service';

export const FORM_STRATEGY = new InjectionToken<FormStrategyModel>('dynamic dialog data');

export interface FormStrategyModel {
  model: BaseFormElementService;
}
