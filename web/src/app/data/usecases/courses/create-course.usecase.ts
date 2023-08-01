import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateCourseUseCase implements UseCase<Course, string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: Course): Observable<string> {
    return this.teacherService.createCourse(item);
  }
}
