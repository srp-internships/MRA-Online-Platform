import { ControlBase, IElement } from '..';

export abstract class PanelBase implements IElement {
  elementType: string = 'panel';
  private readonly _controls!: ControlBase[];
  abstract panelType: string;
  controls(): ControlBase<any>[] {
    return this._controls;
  }
  constructor(...controls: ControlBase[]) {
    this._controls = controls;
  }
}
