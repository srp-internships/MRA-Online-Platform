import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Teacher } from 'src/app/domain/teacher';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { AdminService } from '../../services/admin.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetTeachersUseCase implements UseCase<void, Teacher[]> {
  constructor(private adminService: AdminService) {}

  execute(): Observable<Teacher[]> {
    return this.adminService.getTeachers().pipe(map(x => x.map(t => Object.assign(new Teacher(), t))));
  }
}
