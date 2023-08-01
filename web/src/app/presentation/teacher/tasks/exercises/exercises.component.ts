import { Component, Inject, Input } from '@angular/core';
import { FORM_STRATEGY, FormStrategyModel } from 'src/app/core/form-elements';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { Excercise } from 'src/app/domain/exercise';
import { CRUDBaseComponent } from 'src/app/presentation/crud-base-component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ExerciseUseCasesFacade } from './exercise-usecases.facade';
import { ExercisesFormControlService } from './exercises-control.service';

@Component({
  selector: 'srp-exercises',
  templateUrl: './exercises.component.html',
  styleUrls: ['./exercises.component.scss'],
  providers: [ExerciseUseCasesFacade],
})
export class ExercisesComponent extends CRUDBaseComponent<Excercise> {
  @Input() exercises!: Excercise[];
  constructor(
    private _srpDialogService: SrpDialogService,
    private exerciseUseCasesFacade: ExerciseUseCasesFacade,
    @Inject(FORM_STRATEGY) formStrategy: FormStrategyModel,
    formControlService: ExercisesFormControlService
  ) {
    formStrategy.model = formControlService;
    super(_srpDialogService);
  }
  onAdd(): void {}

  onEdit(item: Excercise): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Excercise(item.themeId), item), {
          dialogTitle: 'Изменить упражнение',
          primaryButtonText: 'Редактировать',
          useCase: this.exerciseUseCasesFacade.editUseCase,
        })
      )

      .subscribe(exercise => {
        this.exercises[this.exercises.findIndex(x => x.id == exercise.id)] = exercise;
      });
  }
  onDelete(exercise: Excercise): void {
    this.dialogService.open(ConfirmDialogComponent, { data: exercise.name }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this.exerciseUseCasesFacade.deleteUseCase.execute(exercise.id).subscribe(() => {
          const index = this.exercises.findIndex(x => x.id == exercise.id);
          this.exercises.splice(index, 1);
        });
      }
    });
  }
}
