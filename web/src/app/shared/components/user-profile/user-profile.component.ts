import { Component } from '@angular/core';
import { DialogConfig, DialogRef } from 'src/app/core/modules/srp-dialog';

@Component({
  selector: 'srp-user-profile',
  templateUrl: './user-profile.component.html',
})
export class UserProfileComponent {
  show: boolean = false;

  constructor(public dialogRef: DialogRef, public config: DialogConfig) {}

  onClose() {
    this.dialogRef.close();
  }

  onChangePassword() {
    this.show = !this.show;
  }
}
