import { IElement } from '..';

export abstract class ControlBase<T = any> implements IElement {
  abstract readonly controlType: string;
  readonly value: T | undefined;
  readonly key: string;
  readonly label: string;
  readonly style: any;
  readonly order: number;
  readonly validators?: any[];
  readonly elementType: string = 'control';
  readonly type: 'text' | 'email' | 'password' | 'date' | 'number';
  readonly options: { value: T; text: string }[];
  constructor(options: Partial<ControlBase<T>> = {}) {
    this.value = options.value;
    this.key = options.key || '';
    this.label = options.label || '';
    this.order = options.order === undefined ? 1 : options.order;
    this.type = options.type || 'text';
    this.options = options.options || [];
    this.validators = options.validators || [];
    this.style = options.style || undefined;
  }

  controls(): ControlBase<T>[] {
    return [this];
  }
}
