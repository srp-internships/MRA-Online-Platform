import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/app/domain';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: DataModulte,
})
export class SignUpUseCase implements UseCase<User, any> {
  constructor(private _authService: AuthService) {}

  execute(item: User): Observable<any> {
    return this._authService.signUp(item);
  }
}
