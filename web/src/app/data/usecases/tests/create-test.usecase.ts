import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Test } from 'src/app/domain/';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateTestUseCase implements UseCase<Test, string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: Test): Observable<string> {
    return this.teacherService.craeteTest(item);
  }
}
