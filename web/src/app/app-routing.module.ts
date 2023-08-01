import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TokenGuard } from './core/guards/token.guard';
import { AppRouterLinks } from './core/routings';
import { NotFoundPageComponent } from './static-pages/not-found-page/not-found-page.component';
import { ProtectedPageComponent } from './static-pages/protected-page/protected-page.component';

const routes: Routes = [
  {
    path: '',
    canActivate: [TokenGuard],
    loadChildren: () => import('./presentation/presentation.module').then(d => d.PresentationModule),
  },
  {
    path: AppRouterLinks.Account,
    loadChildren: () => import('./presentation/account/account.module').then(a => a.AccountModule),
  },
  { path: 'protected-page', component: ProtectedPageComponent },
  { path: '**', component: NotFoundPageComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
