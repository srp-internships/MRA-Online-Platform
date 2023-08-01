import { HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ExerciseResponse } from 'src/app/domain';
import { ProjectExerciseSubmit } from 'src/app/domain/student/project-exercise-submit';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({ providedIn: DataModulte })
export class SubmitProjectExercisesUseCase implements UseCase<ProjectExerciseSubmit, HttpEvent<ExerciseResponse>> {
  constructor(private studentService: StudentService) {}

  execute(exercise: ProjectExerciseSubmit): Observable<HttpEvent<ExerciseResponse>> {
    return this.studentService.submitProjectExercise(exercise);
  }
}
