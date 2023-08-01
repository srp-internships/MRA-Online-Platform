import { Component, OnInit } from '@angular/core';
import { GetStudentCoursesUseCase } from 'src/app/data/usecases/courses/get-courses.usecase';
import { StudentCourse } from 'src/app/domain';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { LayoutService } from 'src/app/layout/layout.service';
import { NavigationService } from 'src/app/core/routings';
import { StudentRouterLinks } from 'src/app/core/routings/modules/student/student.rm';

@Component({
  selector: 'srp-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss'],
})
export class CoursesComponent implements OnInit {
  courses: StudentCourse[] = [];
  constructor(
    private getCourses: GetStudentCoursesUseCase,
    layout: LayoutService,
    private loader: LoaderService,
    private _navigationService: NavigationService
  ) {
    layout.clearSideNav();
  }

  ngOnInit() {
    this.loader.show();
    this.getCourses.execute().subscribe(courses => {
      this.courses = courses;

      this.loader.hide();
    });
  }

  getGuideLink(courseId: string): string {
    return this._navigationService.studentModule(StudentRouterLinks.Guides).resolve({ courseId }).getPath();
  }
}
