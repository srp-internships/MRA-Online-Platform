import { NgModule } from '@angular/core';
import { DynamicDialogModule } from 'src/app/core/modules/dynamic/dialog';
import { SharedModule } from 'src/app/shared/shared.module';
import { TasksComponent } from './tasks.component';
import { TestsComponent } from './tests/tests.component';
import { ExercisesComponent } from './exercises/exercises.component';
import { TasksRoutes } from './tasks.routing';
import { TabsModule } from 'src/app/core/modules/tabs/tabs.module';
import { VariantsDialogComponent } from './tests/variants-dialog/variants-dialog.component';
import { ProjectExercisesComponent } from './project-exercises/project-exercises.component';

@NgModule({
  declarations: [
    TasksComponent,
    TestsComponent,
    ExercisesComponent,
    VariantsDialogComponent,
    ProjectExercisesComponent,
  ],
  imports: [SharedModule, DynamicDialogModule, TabsModule, TasksRoutes],
})
export class TasksModule {}
