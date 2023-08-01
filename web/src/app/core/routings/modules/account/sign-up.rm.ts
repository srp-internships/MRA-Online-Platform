import { ModulePath } from '../../base-path';
import { AccountRouterLinks, AccountRoutingModule } from './account.rm';

export class SignUpRoutingModule extends ModulePath {
  constructor(parentModule: AccountRoutingModule) {
    super(parentModule);
  }
  readonly base: string = AccountRouterLinks.SignUp;

  override getPath(link?: SignUpRouterLinks): string {
    return super.getPath(link);
  }
}

export enum SignUpRouterLinks {
  ConfirmEmail = 'confirm-email',
}
