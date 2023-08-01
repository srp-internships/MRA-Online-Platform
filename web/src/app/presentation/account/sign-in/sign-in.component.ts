import { Component, ErrorHandler, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenManagerService } from 'src/app/core/services/token-storage.service';
import { CustomValidationService } from 'src/app/core/services/custom-validation.service';
import { SignInUseCase } from 'src/app/data/usecases/auth/sign-in.usecase';
import { AccountRouterLinks, AccountRoutingModule } from 'src/app/core/routings';

@Component({
  selector: 'srp-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  loginForm!: FormGroup;
  loading = false;
  submitted = false;
  returnUrl!: string;

  links = AccountRouterLinks;
  constructor(
    private _fb: FormBuilder,
    private _signInUseCase: SignInUseCase,
    private _tokenService: TokenManagerService,
    private _customValidator: CustomValidationService,
    private _router: Router,
    private _route: ActivatedRoute,
    private _errorHandler: ErrorHandler,
    public _accountRotingModule: AccountRoutingModule
  ) {}

  ngOnInit(): void {
    this.loginForm = this._fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.compose([Validators.required, this._customValidator.passwordValidator()])],
    });
    this._tokenService.remove();
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

  get email(): any {
    return this.loginForm.get('email');
  }

  get password(): any {
    return this.loginForm.get('password');
  }

  onSubmit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this._signInUseCase.execute(this.loginForm.value).subscribe({
      next: token => {
        this._tokenService.set(token);
        this._router.navigateByUrl('/');
      },
      error: err => {
        this.loading = false;
        this._errorHandler.handleError(err);
      },
    });
  }
}
