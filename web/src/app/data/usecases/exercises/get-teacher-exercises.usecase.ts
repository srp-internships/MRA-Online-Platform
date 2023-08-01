import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Excercise } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({ providedIn: DataModulte })
export class GetTeacherExercisesUseCase implements UseCase<string, Excercise[]> {
  constructor(private teacherService: TeacherService) {}

  execute(topicId: string): Observable<Excercise[]> {
    return this.teacherService.getExercises(topicId);
  }
}
