import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UseCase } from 'src/app/data/base/use-case';
import { TopicList } from 'src/app/domain';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetStudentTopicsUseCase implements UseCase<string, TopicList[]> {
  constructor(private studentService: StudentService) {}

  execute(courseId: string): Observable<TopicList[]> {
    return this.studentService.getTopicList(courseId);
  }
}
