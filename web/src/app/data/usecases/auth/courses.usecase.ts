import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { List } from 'src/app/domain';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AuthService } from '../../services/auth.service';

@Injectable({
  providedIn: DataModulte,
})
export class AvaliableCoursesUseCase implements UseCase<void, List[]> {
  constructor(private _authService: AuthService) {}

  execute(): Observable<List[]> {
    return this._authService.courses();
  }
}
