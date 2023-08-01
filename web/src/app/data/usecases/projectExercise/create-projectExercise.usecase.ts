import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectExercise } from 'src/app/domain/project-exercise';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateProjectExerciseUseCase implements UseCase<ProjectExercise, string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: ProjectExercise): Observable<string> {
    return this.teacherService.craeteProjectExercise(item);
  }
}
