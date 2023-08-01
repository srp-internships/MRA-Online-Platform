import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Test } from 'src/app/domain';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetTeacherTestsUseCase implements UseCase<string, Test[]> {
  constructor(private teacherService: TeacherService) {}

  execute(topicId: string): Observable<Test[]> {
    return this.teacherService.getTests(topicId);
  }
}
