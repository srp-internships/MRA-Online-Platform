import { ControlBase } from './controls/control-base';

export interface IElement {
  elementType: string;
  controls: () => ControlBase[];
}
