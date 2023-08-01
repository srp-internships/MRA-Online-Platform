import { HttpRequest } from '@angular/common/http';

export interface TokenModel {
  accessToken: string;
  refreshToken: string;
  isPasswordChanged: boolean;
}

export function addTokenHeader(request: HttpRequest<any>, token: string) {
  return request.clone({
    headers: request.headers.set(TOKEN_HEADER_KEY, 'Bearer ' + token),
  });
}

const TOKEN_HEADER_KEY = 'Authorization';
