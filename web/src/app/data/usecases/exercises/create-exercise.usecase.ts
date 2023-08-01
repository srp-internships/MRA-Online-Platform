import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Excercise } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class CreateExerciseUseCase implements UseCase<Excercise, string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: Excercise): Observable<string> {
    return this.teacherService.craeteExercise(item);
  }
}
