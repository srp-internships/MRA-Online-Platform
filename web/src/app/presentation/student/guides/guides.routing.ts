import { RouterModule, Routes } from '@angular/router';
import { GuidContentComponent } from './content/content.component';
import { GuidesComponent } from './guides.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { GuideExercisesComponent } from './exercises/exercises.component';
import ExerciseDeactivateGuard from './exercises/deactivate.guard';
import { ResolvedContentTitle } from './content-title.resolve';
import { GuideRouterLinks } from 'src/app/core/routings/modules/student/guide.rm';
import { StudentTestComponent } from './student-test/student-test.component';
import { StudentProjectExerciseComponent } from './student-project-exercise/student-project-exercise.component';

const routes: Routes = [
  {
    path: '',
    component: GuidesComponent,
    children: [
      {
        path: '',
        title: 'Добро пожаловать!',
        component: WelcomeComponent,
      },
      {
        path: GuideRouterLinks.Topic,
        redirectTo: GuideRouterLinks.TopicContent,
        pathMatch: 'prefix',
      },
      {
        path: GuideRouterLinks.TopicContent,
        title: ResolvedContentTitle,
        component: GuidContentComponent,
      },
      {
        path: GuideRouterLinks.TopicExercises,
        title: 'Задачи',
        canDeactivate: [ExerciseDeactivateGuard],
        component: GuideExercisesComponent,
      },
      {
        path: GuideRouterLinks.TopicTest,
        title: 'Тесты',
        component: StudentTestComponent,
      },
      {
        path: GuideRouterLinks.TopicProjectExercise,
        title: 'Проекты',
        component: StudentProjectExerciseComponent,
      },
    ],
  },
];

export const GuidesRoutes = RouterModule.forChild(routes);
