import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { TokenModel } from 'src/app/domain';

export const auth_key = 'srp_acadeny_auth';

@Injectable({
  providedIn: 'root',
})
export class TokenManagerService {
  constructor(private _jwtHelper: JwtHelperService) {}

  isExpired(accessToken: string): boolean {
    return this._jwtHelper.isTokenExpired(accessToken);
  }

  getRole(token: TokenModel | null = null): string {
    token = token ?? this.getToken();
    return this._jwtHelper.decodeToken(token?.accessToken)['role'];
  }

  getEmail(token: TokenModel | null = null): string {
    token = token ?? this.getToken();
    return this._jwtHelper.decodeToken(token?.accessToken)['email'];
  }

  getToken(): TokenModel | null {
    const token = localStorage.getItem(auth_key);
    if (token) {
      return JSON.parse(atob(token));
    }
    return null;
  }

  set(token: TokenModel): void {
    const hashToken = btoa(JSON.stringify(token));
    localStorage.setItem(auth_key, hashToken);
  }

  remove(): void {
    localStorage.removeItem(auth_key);
  }
}
