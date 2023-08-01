import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountRouterLinks, ResetPassworRouterLinks, NavigationService } from 'src/app/core/routings';
import { CustomValidationService } from 'src/app/core/services/custom-validation.service';
import { AuthService } from 'src/app/data/services/auth.service';
import { ResetPasswordModel } from 'src/app/domain';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';

@Component({
  selector: 'srp-apply-new-password',
  templateUrl: './apply-new-password.component.html',
})
export class ApplyNewPasswordComponent implements OnInit {
  applyingForm!: FormGroup;
  submitted = false;
  loading = false;
  model: ResetPasswordModel = {
    email: '',
    token: '',
    newPassword: '',
  };

  constructor(
    private _fb: FormBuilder,
    private _route: ActivatedRoute,
    private _customValidator: CustomValidationService,
    private authService: AuthService,
    private tostrService: ToastrService,
    private loaderService: LoaderService,
    private _routingFacade: NavigationService
  ) {}

  ngOnInit(): void {
    this.model.email = this._route.snapshot.queryParamMap.get('email')!;
    this.model.token = this._route.snapshot.queryParamMap.get('token')!;
    this.applyingForm = this._fb.group(
      {
        email: [this.model.email, [Validators.required, Validators.email]],
        token: [this.model.token, Validators.required],
        newPassword: ['', Validators.compose([Validators.required, this._customValidator.passwordValidator()])],
        confirmPassword: ['', Validators.required],
      },
      {
        validators: this._customValidator.matchPassword('newPassword', 'confirmPassword'),
      }
    );

    if (this.email?.errors || this.token?.errors) {
      this.tostrService.error('Неверная ссылка. Повторите еще раз!');
      this._routingFacade.resetPasswordModule(ResetPassworRouterLinks.ByEnail).navigate();
    }
  }

  get email() {
    return this.applyingForm.get('email');
  }

  get newPassword(): any {
    return this.applyingForm.get('newPassword');
  }

  get token() {
    return this.applyingForm.get('token');
  }

  get confirmPassword(): any {
    return this.applyingForm.get('confirmPassword');
  }

  get passwordMatchError() {
    return this.applyingForm.getError('mismatch');
  }

  onSubmit() {
    this.submitted = true;

    if (this.applyingForm.invalid) {
      return;
    }
    this.loaderService.show();
    this.model.newPassword = this.applyingForm.value.newPassword;
    this.authService.resetPassword(this.model).subscribe(() => {
      this.loaderService.hide();
      this.tostrService.success('Ваш пароль был успешно изменен!');
      this._routingFacade.accountModule(AccountRouterLinks.SignIn).navigate();
    });
  }
}
