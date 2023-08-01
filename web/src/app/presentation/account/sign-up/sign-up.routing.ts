import { RouterModule, Routes } from '@angular/router';
import { SignUpRouterLinks } from 'src/app/core/routings/modules/account/sign-up.rm';
import { SignUpComponent } from '../sign-up/sign-up.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';

const routes: Routes = [
  { path: '', title: 'Регистрация', component: SignUpComponent },
  { path: SignUpRouterLinks.ConfirmEmail, title: 'Подтверждение электронной почты', component: ConfirmEmailComponent },
];

export const SignUpRoutes = RouterModule.forChild(routes);
