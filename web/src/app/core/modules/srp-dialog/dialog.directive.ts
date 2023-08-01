import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[srpDialogHost]',
})
export class SrpDialogDirective {
  constructor(public viewContainerRef: ViewContainerRef) {}
}
