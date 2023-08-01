import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfirmEmail } from 'src/app/domain';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: DataModulte,
})
export class ConfirmEmailUseCase implements UseCase<ConfirmEmail, any> {
  constructor(private _authService: AuthService) {}

  execute(model: ConfirmEmail): Observable<any> {
    return this._authService.confirmEmail(model);
  }
}
