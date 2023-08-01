import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UseCase } from 'src/app/data/base/use-case';
import { StudentRating } from 'src/app/domain';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetStudentRatingUseCase implements UseCase<string, StudentRating> {
  constructor(private studentService: StudentService) {}

  execute(courseId: string): Observable<StudentRating> {
    return this.studentService.getRates(courseId);
  }
}
