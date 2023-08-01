import { NgModule } from '@angular/core';
import { TeacherComponent } from './teacher.component';
import { TeacherRoutes } from './teacher.routing';
import { SharedModule } from 'src/app/shared/shared.module';
import { CoursesComponent } from './courses/courses.component';
import { TopicsComponent } from './topics/topics.component';
import { DynamicFilterModule } from 'src/app/core/modules/dynamic/filter';
import { DynamicDialogModule } from 'src/app/core/modules/dynamic/dialog';
import { StudentsRatingComponent } from './students-rating/students-rating.component';

@NgModule({
  imports: [SharedModule, TeacherRoutes, DynamicDialogModule, DynamicFilterModule],
  declarations: [TeacherComponent, CoursesComponent, TopicsComponent, StudentsRatingComponent],
})
export class TeacherModule {}
