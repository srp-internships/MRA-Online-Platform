import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { TokenModel } from 'src/app/domain';
import { TokenManagerService } from '../services/token-storage.service';
import { NavigationService } from '../routings/navigation.service';
import { AuthService } from 'src/app/data/services/auth.service';
import { AccountRouterLinks } from '../routings';

@Injectable({
  providedIn: 'root',
})
export class TokenGuard implements CanActivate {
  constructor(
    private _tokenManager: TokenManagerService,
    private _authService: AuthService,
    private _routingFacade: NavigationService
  ) {}

  async canActivate() {
    const token = this._tokenManager.getToken();
    if (!token || !token.refreshToken) {
      this._routingFacade.accountModule(AccountRouterLinks.Callback).navigate();
      return false;
    }
    if (this._tokenManager.isExpired(token.accessToken)) {
      const newToken = await this.tryRefreshingTokens(token);
      this._tokenManager.set(newToken);
    }
    return true;
  }

  private tryRefreshingTokens(token: TokenModel): Promise<TokenModel> {
    return new Promise<TokenModel>((resolve, reject) => {
      this._authService.refreshToken(token).subscribe({
        next: res => resolve(res),
        error: err => {
          reject(err);
        },
      });
    });
  }
}
