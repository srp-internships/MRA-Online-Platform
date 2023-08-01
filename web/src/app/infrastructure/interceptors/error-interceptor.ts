/* eslint-disable prettier/prettier */
import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { switchMap, catchError, filter, take } from 'rxjs/operators';
import { AccountRouterLinks, NavigationService } from 'src/app/core/routings';
import { addTokenHeader, TokenModel } from 'src/app/domain';
import { AuthService } from 'src/app/data/services/auth.service';
import { TokenManagerService } from 'src/app/core/services/token-storage.service';

@Injectable()
export class ErrorCatchingInterceptor implements HttpInterceptor {
  isRefreshing = false;
  refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject(null);
  constructor(private tokenService: TokenManagerService, private authService: AuthService, private _routingFacade: NavigationService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError((err: HttpErrorResponse) => {
      if(err.url?.includes("/refresh")) {
        this.tokenService.remove();
        this._routingFacade.accountModule(AccountRouterLinks.SignIn).navigate();
      }
      if (err.status === 401) {
        return this.handle401Error(req, next);
      }

      return throwError(err);
  }))
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      const token = this.tokenService.getToken();
      if (token)
        return this.authService.refreshToken(token).pipe(
          switchMap((token: TokenModel) => {
            this.isRefreshing = false;
            this.tokenService.set(token);
            this.refreshTokenSubject.next(token.accessToken);

            return next.handle(addTokenHeader(request, token.accessToken));
          }),
          catchError(err => {
            this.isRefreshing = false;
            this.tokenService.remove();
            return throwError(err);
          })
        );
    }
    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token => next.handle(addTokenHeader(request, token)))
    );
  }
}

export const errorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorCatchingInterceptor,
  multi: true,
}
