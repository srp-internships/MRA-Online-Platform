import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class DeleteExerciseUseCase implements UseCase<string, any> {
  constructor(private teacherService: TeacherService) {}

  execute(id: string): Observable<any> {
    return this.teacherService.deleteExercise(id);
  }
}
