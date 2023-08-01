import { ModulePath } from '../../base-path';
import { AccountRouterLinks, AccountRoutingModule } from './account.rm';

export class ResetPassworRoutingModule extends ModulePath {
  constructor(parentModule: AccountRoutingModule) {
    super(parentModule);
  }
  readonly base: string = AccountRouterLinks.ResetPassword;

  override getPath(link?: ResetPassworRouterLinks): string {
    return super.getPath(link);
  }
}

export enum ResetPassworRouterLinks {
  ByEnail = 'by-email',
  Apply = 'apply',
}
