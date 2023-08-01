import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AdminService } from '../../services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class DeleteTeacherUseCase implements UseCase<string, any> {
  constructor(private adminService: AdminService) {}

  execute(id: string): Observable<any> {
    return this.adminService.deleteTeacher(id);
  }
}
