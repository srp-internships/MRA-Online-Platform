import { Component } from '@angular/core';
import { DialogConfig, DialogRef } from 'src/app/core/modules/srp-dialog';

@Component({
  selector: 'srp-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
})
export class ConfirmDialogComponent {
  constructor(public config: DialogConfig, public dialog: DialogRef) {}

  onClose(flag: boolean) {
    this.dialog.close(flag);
  }
}
