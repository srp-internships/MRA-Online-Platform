import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProjectExercise } from 'src/app/domain/project-exercise';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class GetProjectExercisesUseCase implements UseCase<string, ProjectExercise[]> {
  constructor(private teacherService: TeacherService) {}

  execute(topicId: string): Observable<ProjectExercise[]> {
    return this.teacherService.getProjectExercises(topicId);
  }
}
