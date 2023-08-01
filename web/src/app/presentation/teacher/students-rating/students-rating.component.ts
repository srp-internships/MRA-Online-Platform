import { Component, OnInit } from '@angular/core';
import { NavigationService, TeacherRouterLinks } from 'src/app/core/routings';
import { CustomRxJsOperators } from 'src/app/core/services/custom-rxjs-operators.service';
import { GetTeacherCoursesUseCase } from 'src/app/data/usecases/courses/get-teacher-courses.usecase';
import { GetStudentsRatingUseCase } from 'src/app/data/usecases/ratings/get-students-rating.usecase';
import { List, StudentsRating } from 'src/app/domain';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';

@Component({
  selector: 'srp-students-rating',
  templateUrl: './students-rating.component.html',
  styleUrls: ['./students-rating.component.scss'],
})
export class StudentsRatingComponent implements OnInit {
  items!: StudentsRating[];
  filteredItems!: StudentsRating[];
  courses!: List[];
  selectedCourseId!: string;
  constructor(
    private getStudentsRatingUseCase: GetStudentsRatingUseCase,
    private getCoursesUseCase: GetTeacherCoursesUseCase,
    private customOperators: CustomRxJsOperators,
    private loader: LoaderService,
    private _navigationFactory: NavigationService
  ) {}

  ngOnInit() {
    this.getCoursesUseCase
      .execute()
      .pipe(
        this.customOperators.navigateIfEmpty(
          this._navigationFactory.teacherModule(TeacherRouterLinks.Courses),
          `У вас нет курсов для загрузки. Сначала создайте курс!`
        )
      )
      .subscribe(courses => {
        this.courses = courses;
        this.selectedCourseId = this.courses[0].id;
        this.loadRatings();
      });
  }

  loadRatings() {
    this.loader.show();
    this.getStudentsRatingUseCase.execute(this.selectedCourseId).subscribe(ratings => {
      this.items = ratings;
      this.loader.hide();
    });
  }
}
