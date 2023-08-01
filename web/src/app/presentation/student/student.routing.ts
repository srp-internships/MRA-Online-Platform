import { Routes, RouterModule } from '@angular/router';
import { StudentRouterLinks } from 'src/app/core/routings/modules/student/student.rm';
import { CoursesComponent } from './courses/courses.component';
import { StudentComponent } from './student.component';

const routes: Routes = [
  {
    path: '',
    component: StudentComponent,
    children: [
      {
        path: '',
        redirectTo: StudentRouterLinks.Courses,
        pathMatch: 'full',
      },
      {
        path: StudentRouterLinks.Courses,
        title: 'Курсы',
        component: CoursesComponent,
      },
      {
        path: StudentRouterLinks.Guides,
        title: 'Руководоств',
        loadChildren: () => import('./guides/guides.module').then(d => d.GuidesModule),
      },
    ],
  },
];

export const StudentRoutes = RouterModule.forChild(routes);
