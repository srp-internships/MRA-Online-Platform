import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StudentExercise } from 'src/app/domain/index';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({ providedIn: DataModulte })
export class GetStudentExercisesUseCase implements UseCase<string, StudentExercise[]> {
  constructor(private studentService: StudentService) {}

  execute(topicId: string): Observable<StudentExercise[]> {
    return this.studentService.getExercises(topicId);
  }
}
