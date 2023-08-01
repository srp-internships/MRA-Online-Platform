import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ExerciseResponse, ExerciseSubmit } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({
  providedIn: DataModulte,
})
export class CheckExerciseUseCase implements UseCase<ExerciseSubmit, ExerciseResponse> {
  constructor(private studentService: StudentService) {}

  execute(item: ExerciseSubmit): Observable<ExerciseResponse> {
    // return of({ success: true, errors: 'Test error' }).pipe(delay(1500));
    return this.studentService.checkExercise(item);
  }
}
