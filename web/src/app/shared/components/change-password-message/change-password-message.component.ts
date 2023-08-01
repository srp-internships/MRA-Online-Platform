import { Component } from '@angular/core';
import { Toast, ToastPackage, ToastrService } from 'ngx-toastr';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { ChangePasswordComponent } from '../change-password/change-password.component';

@Component({
  selector: 'srp-change-password-message',
  templateUrl: './change-password-message.component.html',
  styleUrls: ['./change-password-message.component.scss'],
  preserveWhitespaces: false,
})
export class ChangePasswordMessageComponent extends Toast {
  showing: boolean = false;
  constructor(
    private dialogService: SrpDialogService,
    protected override toastrService: ToastrService,
    public override toastPackage: ToastPackage
  ) {
    super(toastrService, toastPackage);
  }

  onChangePassword() {
    this.dialogService.open(ChangePasswordComponent);
  }

  onShow() {
    localStorage.setItem('srp_settings', JSON.stringify({ disable_ch_p: true }));
  }
}
