import { Injectable } from '@angular/core';
import { CanLoad, Route, Router, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountRouterLinks, AccountRoutingModule } from '../routings';
import { TokenManagerService } from '../services/token-storage.service';

@Injectable({
  providedIn: 'root',
})
export class RoleGuard implements CanLoad {
  constructor(
    private _tokenManager: TokenManagerService,
    private router: Router,
    private _accountRouting: AccountRoutingModule
  ) {}

  canLoad(
    route: Route,
    // eslint-disable-next-line unused-imports/no-unused-vars
    segments: UrlSegment[]
  ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const token = this._tokenManager.getToken();
    if (token) {
      const role = this._tokenManager.getRole(token);
      if (route.data!['roles'] && !route.data!['roles'].includes(role)) {
        this.router.navigate(['/protected-page']);
        return false;
      }
      return true;
    }
    this.router.navigate([this._accountRouting.getPath(AccountRouterLinks.SignIn)]);
    return false;
  }
}
