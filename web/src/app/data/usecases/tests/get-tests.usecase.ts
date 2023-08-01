import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StudentTest } from 'src/app/domain/student/student-test';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({ providedIn: DataModulte })
export class GetStudentTestsUseCase implements UseCase<string, StudentTest[]> {
  constructor(private studentService: StudentService) {}

  execute(topicId: string): Observable<StudentTest[]> {
    return this.studentService.getTests(topicId);
  }
}
