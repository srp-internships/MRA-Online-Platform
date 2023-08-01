import { Routes, RouterModule } from '@angular/router';
import { TeacherRouterLinks } from 'src/app/core/routings';
import { CoursesComponent } from './courses/courses.component';
import { StudentsRatingComponent } from './students-rating/students-rating.component';
import { TeacherComponent } from './teacher.component';
import { TopicsComponent } from './topics/topics.component';

const routes: Routes = [
  {
    path: '',
    component: TeacherComponent,
    children: [
      {
        path: '',
        redirectTo: 'courses',
        pathMatch: 'full',
      },
      {
        path: TeacherRouterLinks.Courses,
        title: 'Курсы',
        component: CoursesComponent,
      },
      {
        path: TeacherRouterLinks.Topics,
        title: 'Темы',
        component: TopicsComponent,
      },
      {
        path: TeacherRouterLinks.Tasks,
        title: 'Задачи',
        loadChildren: () => import('./tasks/tasks.module').then(t => t.TasksModule),
      },
      {
        path: TeacherRouterLinks.StudentRatings,
        title: 'Рейтинги студентов',
        component: StudentsRatingComponent,
      },
    ],
  },
];

export const TeacherRoutes = RouterModule.forChild(routes);
