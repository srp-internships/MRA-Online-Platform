import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel, TokenModel } from 'src/app/domain';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: DataModulte,
})
export class SignInUseCase implements UseCase<LoginModel, TokenModel> {
  constructor(private _authService: AuthService) {}

  execute(model: LoginModel): Observable<TokenModel> {
    return this._authService.signIn(model);
  }
}
