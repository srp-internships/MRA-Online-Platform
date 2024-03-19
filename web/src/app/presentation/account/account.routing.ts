import { Routes, RouterModule } from '@angular/router';
import { AccountRouterLinks } from 'src/app/core/routings';
import { AccountComponent } from './account.component';
import {CallbackComponent} from "./callback/callback.component";

const routes: Routes = [
  {
    path: '',
    component: AccountComponent,
    children: [
      {
        path: '',
        redirectTo: AccountRouterLinks.Callback,
        pathMatch: 'full',
      },
      {
        path: AccountRouterLinks.Callback,
        title: 'callback',
        component: CallbackComponent,
      },
    ],
  },
];

export const AccountRoutes = RouterModule.forChild(routes);
