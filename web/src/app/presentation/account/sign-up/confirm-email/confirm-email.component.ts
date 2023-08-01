import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';
import { ConfirmEmailUseCase } from 'src/app/data/usecases/auth/confirm-email.usecase';

@Component({
  selector: 'srp-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {
  loading: boolean = false;
  email: string | null = '';
  constructor(
    private _route: ActivatedRoute,
    private _confirmEmailUseCase: ConfirmEmailUseCase,
    private _toastr: ToastrService,
    private _routingFacade: NavigationService
  ) {}

  ngOnInit(): void {
    this.email = this._route.snapshot.queryParamMap.get('email');
    const token = this._route.snapshot.queryParamMap.get('token');
    const form = new UntypedFormGroup({
      email: new UntypedFormControl(this.email, [Validators.required, Validators.email]),
      token: new UntypedFormControl(token, Validators.required),
    });

    if (!form.valid) {
      this._toastr.error('Неверная ссылка. Повторите еще раз!');
      this._routingFacade.accountModule(AccountRouterLinks.SignUp).navigate();
      return;
    }

    this.loading = true;
    this._confirmEmailUseCase
      .execute(form.value)
      .pipe(finalize(() => (this.loading = false)))
      .subscribe(() => {
        this._toastr.success('Регистрация успешно завершена.');
        this._routingFacade.accountModule(AccountRouterLinks.SignIn).navigate();
      });
  }
}
