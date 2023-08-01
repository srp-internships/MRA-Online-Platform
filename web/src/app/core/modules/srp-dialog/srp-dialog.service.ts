import {
  Injectable,
  ComponentFactoryResolver,
  ApplicationRef,
  Injector,
  Type,
  EmbeddedViewRef,
  ComponentRef,
} from '@angular/core';
import { DialogInjector } from './dialog-injector';
import { DialogConfig } from './dialog-config';
import { DialogRef } from './dialog-ref';
import { SrpDialogComponent } from './srp-dialog.component';

@Injectable({
  providedIn: 'root',
})
export class SrpDialogService {
  dialogComponentRef!: ComponentRef<SrpDialogComponent>;
  dialogRef!: DialogRef | null;
  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private appRef: ApplicationRef,
    private injector: Injector
  ) {}

  public open<T>(componentType: Type<any>, config?: DialogConfig<T>) {
    if (!this.dialogRef) {
      this.dialogRef = this.appendDialogComponentToBody<T>(config);
      this.dialogComponentRef.instance.childComponentType = componentType;
      this.dialogComponentRef.instance.isModal = config?.modal;
    }
    return this.dialogRef;
  }

  private appendDialogComponentToBody<T>(config?: DialogConfig<T>) {
    const map = new WeakMap();
    map.set(DialogConfig<T>, config ?? {});

    const dialogRef = new DialogRef();
    map.set(DialogRef, dialogRef);

    const sub = dialogRef.afterClosed.subscribe(() => {
      this.removeDialogComponentFromBody();
      sub.unsubscribe();
    });
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(SrpDialogComponent);
    const componentRef = componentFactory.create(new DialogInjector(this.injector, map));

    this.appRef.attachView(componentRef.hostView);

    const domElem = (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0] as HTMLElement;
    document.body.appendChild(domElem);

    this.dialogComponentRef = componentRef;

    this.dialogComponentRef.instance.onClose.subscribe(() => {
      this.removeDialogComponentFromBody();
    });

    return dialogRef;
  }

  private removeDialogComponentFromBody() {
    this.appRef.detachView(this.dialogComponentRef.hostView);
    this.dialogComponentRef.destroy();
    this.dialogRef = null;
  }
}
