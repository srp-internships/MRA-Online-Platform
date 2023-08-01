import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/data/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CustomValidationService } from 'src/app/core/services/custom-validation.service';
import { TokenManagerService } from 'src/app/core/services/token-storage.service';
import { DialogConfig, DialogRef } from 'src/app/core/modules/srp-dialog';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';

@Component({
  selector: 'srp-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent implements OnInit {
  form!: FormGroup;
  submitted = false;

  constructor(
    public dialogRef: DialogRef,
    public config: DialogConfig,
    private authService: AuthService,
    private _customValidator: CustomValidationService,
    private tostrService: ToastrService,
    private tokenManager: TokenManagerService,
    private _navigationFactory: NavigationService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup(
      {
        currentPassword: new FormControl(
          '',
          Validators.compose([Validators.required, this._customValidator.passwordValidator()])
        ),
        newPassword: new FormControl(
          '',
          Validators.compose([Validators.required, this._customValidator.passwordValidator()])
        ),
        confirmPassword: new FormControl('', Validators.required),
      },
      this._customValidator.matchPassword('newPassword', 'confirmPassword')
    );
  }

  get email(): any {
    return this.form.get('email');
  }

  get currentPassword(): any {
    return this.form.get('currentPassword');
  }

  get newPassword(): any {
    return this.form.get('newPassword');
  }

  get confirmPassword(): any {
    return this.form.get('confirmPassword');
  }

  get passwordMatchError() {
    return this.form.getError('mismatch') && this.form.get('confirmPassword')?.touched;
  }

  onSubmit() {
    this.submitted = true;

    if (this.form.invalid) {
      return;
    }
    this.form.value.email = this.tokenManager.getEmail();
    this.authService.changePassword(this.form.value).subscribe(() => {
      this.tostrService.success('Пароль изменён!');
      this.dialogRef.close();
      this._navigationFactory.accountModule(AccountRouterLinks.SignIn).navigate();
    });
  }
  onClose() {
    this.dialogRef.close();
  }
}
