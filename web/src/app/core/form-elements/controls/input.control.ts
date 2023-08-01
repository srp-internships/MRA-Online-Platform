import { ControlBase } from './control-base';

export class InputControl<T = string> extends ControlBase<T> {
  controlType: string = 'input';
}
