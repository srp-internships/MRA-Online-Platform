import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Course } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetTeacherCoursesUseCase implements UseCase<void, Course[]> {
  constructor(private teacherService: TeacherService) {}

  execute(): Observable<Course[]> {
    return this.teacherService.getCourses();
  }
}
