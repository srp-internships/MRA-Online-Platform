import { Component } from '@angular/core';
import { TeacherRouterLinks, TeacherRoutingModule } from 'src/app/core/routings';
import { LayoutService } from 'src/app/layout/layout.service';

@Component({
  selector: 'srp-teacher',
  templateUrl: './teacher.component.html',
  styleUrls: ['./teacher.component.scss'],
})
export class TeacherComponent {
  constructor(layout: LayoutService, teacherRouting: TeacherRoutingModule) {
    layout.setSideNavItems([
      {
        title: 'Меню',
        items: [
          {
            link: teacherRouting.getPath(TeacherRouterLinks.Courses),
            text: 'Курсы',
            icon: 'fas fa-list',
            disabled: false,
          },
          {
            link: teacherRouting.getPath(TeacherRouterLinks.Topics),
            text: 'Темы',
            icon: 'fas fa-list-ol',
            disabled: false,
          },
        ],
      },
      {
        title: 'Рейтинги ⭐',
        items: [
          {
            link: teacherRouting.getPath(TeacherRouterLinks.StudentRatings),
            text: 'Студенты',
            disabled: false,
          },
        ],
      },
    ]);
  }
}
