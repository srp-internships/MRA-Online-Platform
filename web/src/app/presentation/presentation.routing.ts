import { Routes, RouterModule } from '@angular/router';
import { RoleGuard } from '../core/guards/role.guard';
import { AppRouterLinks } from '../core/routings';
import { Roles } from '../domain';
import { HelpComponent } from './common-pages/help/help.component';
import { PresentationComponent } from './presentation.component';

const routes: Routes = [
  {
    path: '',
    component: PresentationComponent,
    children: [
      {
        path: AppRouterLinks.Admin,
        canLoad: [RoleGuard],
        loadChildren: () => import('./admin/admin.module').then(d => d.AdminModule),
        data: {
          roles: [Roles.Admin],
        },
      },
      {
        path: AppRouterLinks.Student,
        canLoad: [RoleGuard],
        loadChildren: () => import('./student/student.module').then(d => d.StudentModule),
        data: {
          roles: [Roles.Student],
        },
      },
      {
        path: AppRouterLinks.Teacher,
        canLoad: [RoleGuard],
        loadChildren: () => import('./teacher/teacher.module').then(m => m.TeacherModule),
        data: {
          roles: [Roles.Teacher],
        },
      },
      {
        path: 'help',
        component: HelpComponent,
      },
    ],
  },
];

export const PresentationRoutes = RouterModule.forChild(routes);
