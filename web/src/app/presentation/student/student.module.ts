import { NgModule } from '@angular/core';
import { StudentComponent } from './student.component';
import { StudentRoutes } from './student.routing';
import { CoursesComponent } from './courses/courses.component';
import { SharedModule } from 'src/app/shared/shared.module';
import ExerciseDeactivateGuard from './guides/exercises/deactivate.guard';

@NgModule({
  imports: [SharedModule, StudentRoutes],
  providers: [ExerciseDeactivateGuard],
  declarations: [StudentComponent, CoursesComponent],
})
export class StudentModule {}
