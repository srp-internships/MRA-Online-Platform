import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Topic } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetStudentTopicUseCase implements UseCase<string, Topic> {
  constructor(private studentService: StudentService) {}

  execute(params: string): Observable<Topic> {
    return this.studentService.getTopicById(params);
  }
}
