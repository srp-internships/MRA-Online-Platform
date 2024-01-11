import { Routes, RouterModule } from '@angular/router';
import { ComingSoonLinks } from '../../core/routings/modules/ComingSoon/ComingSoon.rm';
import { ComingSoon } from './ComingSoon';

const routes: Routes = [
    {
        path: '',
        component: ComingSoon
    },
];

export const ComingSoonRoutes = RouterModule.forChild(routes);
