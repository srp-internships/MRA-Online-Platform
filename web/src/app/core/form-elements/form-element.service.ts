import { Observable } from 'rxjs';
import { IElement } from '.';

export abstract class BaseFormElementService<T = any> {
  abstract elements(model: T | null): IElement[] | Observable<IElement[]>;
}
