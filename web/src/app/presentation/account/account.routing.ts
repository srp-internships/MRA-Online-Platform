import { Routes, RouterModule } from '@angular/router';
import { AccountRouterLinks } from 'src/app/core/routings';
import { AccountComponent } from './account.component';
import { CheckEmailMessageComponent } from './check-email-message/check-email-message.component';
import { SignInComponent } from './sign-in/sign-in.component';

const routes: Routes = [
  {
    path: '',
    component: AccountComponent,
    children: [
      {
        path: '',
        redirectTo: AccountRouterLinks.SignIn,
        pathMatch: 'full',
      },
      {
        path: AccountRouterLinks.SignIn,
        title: 'Войти',
        component: SignInComponent,
      },
      {
        path: AccountRouterLinks.SignUp,
        title: 'Регистрация',
        loadChildren: () => import('./sign-up/sign-up.module').then(x => x.SignUpModule),
      },
      {
        path: AccountRouterLinks.ResetPassword,
        title: 'Сброс пароля',
        loadChildren: () => import('./reset-password/reset-password.module').then(x => x.ResetPasswordModule),
      },
      {
        path: AccountRouterLinks.CheckEmail,
        component: CheckEmailMessageComponent,
      },
    ],
  },
];

export const AccountRoutes = RouterModule.forChild(routes);
