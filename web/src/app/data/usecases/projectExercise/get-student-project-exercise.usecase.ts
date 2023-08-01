import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StudentProjectExercise } from 'src/app/domain/student/student-project-exercise';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { StudentService } from '../../services/student.service';

@Injectable({ providedIn: DataModulte })
export class GetStudentProjectExercisesUseCase implements UseCase<string, StudentProjectExercise[]> {
  constructor(private studentService: StudentService) {}

  execute(topicId: string): Observable<StudentProjectExercise[]> {
    return this.studentService.getProjectExercises(topicId);
  }
}
