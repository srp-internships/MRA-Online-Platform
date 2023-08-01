import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TestResponse } from 'src/app/domain/student/test-response';
import { TestSubmit } from 'src/app/domain/student/test-submit';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class CheckTestUseCase implements UseCase<TestSubmit, TestResponse> {
  constructor(private studentService: StudentService) {}

  execute(item: TestSubmit): Observable<TestResponse> {
    return this.studentService.checkTest(item);
  }
}
