import { Routes, RouterModule } from '@angular/router';
import { AdminRouterLinks } from '../../core/routings/modules/admin/admin.rm';
import { AdminComponent } from './admin.component';
import { DocumentationComponent } from './documentation/documentation.component';
import { TeachersComponent } from './teachers/teachers.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    children: [
      {
        path: '',
        redirectTo: AdminRouterLinks.Teachers,
        pathMatch: 'full',
      },
      {
        path: AdminRouterLinks.Teachers,
        title: 'Страница учителей',
        component: TeachersComponent,
      },
      {
        path: AdminRouterLinks.Docs,
        title: 'Документация',
        component: DocumentationComponent,
      },
    ],
  },
];

export const AdminRoutes = RouterModule.forChild(routes);
