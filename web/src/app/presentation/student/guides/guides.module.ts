import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { GuidesComponent } from './guides.component';
import { GuidesRoutes } from './guides.routing';
import { GuidContentComponent } from './content/content.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { GuideExercisesComponent } from './exercises/exercises.component';
import { CodemirrorModule } from '@ctrl/ngx-codemirror';
import { ResolvedContentTitle } from './content-title.resolve';
import { StudentTestComponent } from './student-test/student-test.component';
import { StudentProjectExerciseComponent } from './student-project-exercise/student-project-exercise.component';
import { FileUploadService } from '../../../core/services/file-upload.service';

@NgModule({
  declarations: [
    GuidesComponent,
    GuideExercisesComponent,
    GuidContentComponent,
    WelcomeComponent,
    StudentTestComponent,
    StudentProjectExerciseComponent,
  ],
  imports: [SharedModule, GuidesRoutes, CodemirrorModule],
  providers: [ResolvedContentTitle, FileUploadService],
})
export class GuidesModule {}
