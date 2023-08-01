import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Topic } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class EditTopicUseCase implements UseCase<Topic, string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: Topic): Observable<string> {
    return this.teacherService.editTopic(item);
  }
}
