import { Component, Inject, OnInit } from '@angular/core';
import { Teacher } from 'src/app/domain/teacher';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { CRUDBaseComponent } from '../../crud-base-component';
import { TeachersFormControlService, TeachersPostFormControlService } from './teachers-control.service';
import { TeachersUseCasesFacade } from './teachers-usecases.facade';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';

@Component({
  selector: 'srp-teachers',
  templateUrl: './teachers.component.html',
  providers: [TeachersUseCasesFacade, TeachersFormControlService, TeachersPostFormControlService],
  styleUrls: ['./teachers.component.scss'],
})
export class TeachersComponent extends CRUDBaseComponent<Teacher> implements OnInit {
  filteredTeachers!: Teacher[];
  constructor(
    _dialog: SrpDialogService,
    private teacherUseCasesFacade: TeachersUseCasesFacade,
    private loader: LoaderService,
    private formControlService: TeachersFormControlService,
    private postFormControlService: TeachersPostFormControlService,
    @Inject(FORM_STRATEGY) private formStrategy: FormStrategyModel
  ) {
    formStrategy.model = formControlService;
    super(_dialog);
  }
  ngOnInit(): void {
    this.loader.show();
    this.teacherUseCasesFacade.getUseCase.execute().subscribe(teachers => {
      this.items = teachers;
      this.loader.hide();
    });
  }

  onAdd(): void {
    this.formStrategy.model = this.postFormControlService;
    super
      .openDynamicDailog(
        new DynamicDialogData(new Teacher(), { dialogTitle: 'Учетные данные', primaryButtonText: 'Следующий' })
      )
      .subscribe(credential => {
        this.formStrategy.model = this.formControlService;
        super
          .openDynamicDailog(
            new DynamicDialogData(credential, {
              dialogTitle: 'Новый учитель',
              useCase: this.teacherUseCasesFacade.createUseCase,
              primaryButtonText: 'Сохранить',
            })
          )
          .subscribe(teacher => {
            this.items.push(teacher);
          });
      });
  }
  onEdit(item: Teacher): void {
    this.formStrategy.model = this.formControlService;
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Teacher(), item), {
          dialogTitle: 'Изменить учителя',
          useCase: this.teacherUseCasesFacade.editUseCase,
          primaryButtonText: 'Редактировать',
        })
      )
      .subscribe(course => {
        this.items[this.items.findIndex(x => x.id == course.id)] = course;
      });
  }
  onDelete(item: Teacher): void {
    this.dialogService.open(ConfirmDialogComponent, { data: item.email }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this.teacherUseCasesFacade.deleteUseCase.execute(item.id).subscribe(() => {
          const index = this.items.findIndex(x => x.id == item.id);
          this.items.splice(index, 1);
        });
      }
    });
  }
}
