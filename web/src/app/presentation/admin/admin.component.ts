import { Component } from '@angular/core';
import { AdminRouterLinks, AdminRoutingModule } from 'src/app/core/routings/modules/admin/admin.rm';
import { LayoutService } from 'src/app/layout/layout.service';

@Component({
  selector: 'srp-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent {
  constructor(layout: LayoutService, adminRouting: AdminRoutingModule) {
    layout.setSideNavItems([
      {
        title: 'Меню',
        items: [
          {
            link: adminRouting.getPath(AdminRouterLinks.Teachers),
            text: 'Учителя',
            icon: 'fas fa-chalkboard-user',
            disabled: false,
          },
          {
            link: adminRouting.getPath(AdminRouterLinks.Docs),
            text: 'Документация',
            icon: 'fas fa-book',
            disabled: false,
          },
        ],
      },
    ]);
  }
}
