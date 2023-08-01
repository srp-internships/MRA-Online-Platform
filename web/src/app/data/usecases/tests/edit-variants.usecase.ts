import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Variant } from 'src/app/domain/variant';
import { UseCase } from '../../base/use-case';
import { DataModulte } from '../../data.module';
import { TeacherService } from '../../services/teacher.service';

@Injectable({
  providedIn: DataModulte,
})
export class EditVariantsUseCase implements UseCase<Variant[], string> {
  constructor(private teacherService: TeacherService) {}

  execute(item: Variant[]): Observable<string> {
    return this.teacherService.editVariants(item);
  }
}
