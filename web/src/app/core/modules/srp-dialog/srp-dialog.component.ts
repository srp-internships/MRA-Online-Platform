import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ComponentFactoryResolver,
  ComponentRef,
  OnDestroy,
  Type,
  ViewChild,
} from '@angular/core';
import { Subject } from 'rxjs';
import { SrpDialogDirective } from './dialog.directive';

@Component({
  selector: 'srp-srp-dialog',
  templateUrl: './srp-dialog.component.html',
  styleUrls: ['./srp-dialog.component.scss'],
})
export class SrpDialogComponent implements AfterViewInit, OnDestroy {
  componentRef!: ComponentRef<any>;
  isModal?: boolean;
  @ViewChild(SrpDialogDirective)
  insertionPoint!: SrpDialogDirective;

  private readonly _onClose = new Subject<any>();
  public onClose = this._onClose.asObservable();

  childComponentType!: Type<any>;

  constructor(private componentFactoryResolver: ComponentFactoryResolver, private cd: ChangeDetectorRef) {}

  ngAfterViewInit() {
    this.loadChildComponent(this.childComponentType);
    this.cd.detectChanges();
  }

  loadChildComponent(componentType: Type<any>) {
    let componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentType);

    let viewContainerRef = this.insertionPoint.viewContainerRef;
    viewContainerRef.clear();

    this.componentRef = viewContainerRef.createComponent(componentFactory);
  }

  ngOnDestroy() {
    if (this.componentRef) {
      this.componentRef.destroy();
    }
  }

  close() {
    this._onClose.next(null);
  }

  onClickBackground() {
    if (!this.isModal) {
      this.close();
    }
  }
}
