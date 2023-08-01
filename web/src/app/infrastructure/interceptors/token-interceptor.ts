/* eslint-disable prettier/prettier */
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { addTokenHeader } from 'src/app/domain';
import { BASE_API_URL } from 'src/app/injectors';
import { TokenManagerService } from 'src/app/core/services/token-storage.service';


@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private tokenService: TokenManagerService, @Inject(BASE_API_URL) private apiUrl: string) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<Object>> {
    let authReq = req;
    const token = this.tokenService.getToken();
    const isApiUrl = req.url.startsWith(this.apiUrl);
    if (token != null && isApiUrl) {
      authReq = addTokenHeader(req, token.accessToken);
    }
    return next.handle(authReq);
  }
}

export const tokenInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: TokenInterceptor,
  multi: true,
};
