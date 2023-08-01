import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UseCase } from 'src/app/data/base/use-case';
import { StudentsRating } from 'src/app/domain';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetStudentsRatingUseCase implements UseCase<string, StudentsRating[]> {
  constructor(private teacherService: TeacherService) {}

  execute(courseId: string): Observable<StudentsRating[]> {
    return this.teacherService.getStudnetsRating(courseId);
  }
}
