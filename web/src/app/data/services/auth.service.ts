import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseService } from 'src/app/data/base/base-service';
import {
  ChangePasswordModel,
  ConfirmEmail,
  List,
  LoginModel,
  ResetPasswordModel,
  TokenModel,
  User,
} from 'src/app/domain';
import { BASE_API_URL } from 'src/app/injectors';

@Injectable({
  providedIn: 'root',
})
export class AuthService extends BaseService {
  segment: string = 'account';
  constructor(@Inject(BASE_API_URL) apiUrl: string, _http: HttpClient) {
    super(apiUrl, _http);
  }

  signIn(login: LoginModel): Observable<TokenModel> {
    return this._http.post<TokenModel>(this.getPath() + 'signin', login);
  }

  signUp(user: User): Observable<any> {
    return this._http.post<any>(this.getPath() + 'signup', user);
  }

  confirmEmail(model: ConfirmEmail) {
    return this._http.post<any>(this.getPath() + 'confirmemail', model);
  }

  courses(): Observable<List[]> {
    return this._http.get<List[]>(this.getPath() + 'courses');
  }

  logOut() {}

  refreshToken(token: TokenModel): Observable<TokenModel> {
    return this._http.post<TokenModel>(this.getPath() + 'refresh', { token });
  }

  getRoles(): Observable<string[]> {
    return this._http.get<string[]>(this.getPath() + 'roles');
  }

  changePassword(model: ChangePasswordModel): Observable<any> {
    return this._http.put<any>(this.getPath() + 'changePassword', model);
  }

  forgotPassword(email: string) {
    return this._http.post(this.getPath() + 'forgotpassword', { email });
  }

  resetPassword(model: ResetPasswordModel) {
    return this._http.post(this.getPath() + 'resetPassword', model);
  }
}
