import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StudentCourse } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetStudentCoursesUseCase implements UseCase<void, StudentCourse[]> {
  constructor(private studentServise: StudentService) {}

  execute(): Observable<StudentCourse[]> {
    return this.studentServise.getCourses();
  }
}
