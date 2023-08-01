import { Component, Inject, OnInit } from '@angular/core';
import { CoursesFormControlService } from './courses-control.service';
import { CRUDBaseComponent } from '../../crud-base-component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { Course } from 'src/app/domain/course';
import { CourseUseCasesFacade } from './course-usecases.facade';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';

@Component({
  selector: 'srp-courses',
  templateUrl: './courses.component.html',
  providers: [CoursesFormControlService, CourseUseCasesFacade],
  styleUrls: ['./courses.component.scss'],
})
export class CoursesComponent extends CRUDBaseComponent<Course> implements OnInit {
  filteredCourses!: Course[];
  constructor(
    private _courseUseCaseFacade: CourseUseCasesFacade,
    private loader: LoaderService,
    _dialogService: SrpDialogService,
    formControlService: CoursesFormControlService,
    @Inject(FORM_STRATEGY) formStrategy: FormStrategyModel
  ) {
    formStrategy.model = formControlService;
    super(_dialogService);
  }

  ngOnInit(): void {
    this.loader.show();
    this._courseUseCaseFacade.getUseCase.execute().subscribe(courses => {
      this.items = courses;
      this.loader.hide();
    });
  }

  onAdd(): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(new Course(), {
          dialogTitle: 'Новый курс',
          primaryButtonText: 'Сохранить',
          useCase: this._courseUseCaseFacade.createUseCase,
        })
      )
      .subscribe(course => {
        this.items.push(course);
      });
  }
  onEdit(course: Course): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Course(), course), {
          dialogTitle: 'Изменить курс',
          primaryButtonText: 'Редактировать',
          useCase: this._courseUseCaseFacade.editUseCase,
        })
      )
      .subscribe(course => {
        this.items[this.items.findIndex(x => x.id == course.id)] = course;
      });
  }

  onDelete(course: Course): void {
    this.dialogService.open(ConfirmDialogComponent, { data: course.name }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this._courseUseCaseFacade.deleteUseCase.execute(course.id).subscribe(() => {
          const index = this.items.findIndex(x => x.id == course.id);
          this.items.splice(index, 1);
        });
      }
    });
  }
}
