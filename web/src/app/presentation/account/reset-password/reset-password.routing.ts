import { Routes, RouterModule } from '@angular/router';
import { ByEmailComponent } from './by-email/by-email.component';
import { ApplyNewPasswordComponent } from './apply/apply-new-password.component';
import { ResetPassworRouterLinks } from 'src/app/core/routings';

const routes: Routes = [
  {
    path: '',
    redirectTo: ResetPassworRouterLinks.ByEnail,
    pathMatch: 'full',
  },
  {
    path: ResetPassworRouterLinks.ByEnail,
    component: ByEmailComponent,
  },
  {
    path: ResetPassworRouterLinks.Apply,
    component: ApplyNewPasswordComponent,
  },
];

export const ResetPasswordRoutes = RouterModule.forChild(routes);
