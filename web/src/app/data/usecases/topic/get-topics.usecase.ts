import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Topic } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetTeacherTopicsUseCase implements UseCase<string, Topic[]> {
  constructor(private teacherService: TeacherService) {}

  execute(courseId: string): Observable<Topic[]> {
    return this.teacherService.getTopics(courseId);
  }
}
