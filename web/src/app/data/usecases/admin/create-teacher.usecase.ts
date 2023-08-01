import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Teacher } from 'src/app/domain/teacher';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AdminService } from '../../services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateTeacherUseCase implements UseCase<Teacher, string> {
  constructor(private adminService: AdminService) {}

  execute(item: Teacher): Observable<string> {
    return this.adminService.createTeacher(item);
  }
}
