import { AppRouterLinks } from '../app-router-links';
import { ModulePath } from '../../base-path';
import { ResetPassworRoutingModule } from './reset-passwrod.rm';
import { Injectable } from '@angular/core';
import { SignUpRoutingModule } from './sign-up.rm';

@Injectable({ providedIn: 'root' })
export class AccountRoutingModule extends ModulePath {
  readonly base: string = AppRouterLinks.Account;
  readonly resetPassword = new ResetPassworRoutingModule(this);
  readonly signUp = new SignUpRoutingModule(this);

  override getPath(link?: AccountRouterLinks): string {
    return super.getPath(link);
  }
}

export enum AccountRouterLinks {
  SignIn = 'sign-in',
  SignUp = 'sign-up',
  ResetPassword = 'reset-password',
  CheckEmail = 'check-email',
}
