import { Component, Input } from '@angular/core';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { ProjectExercise } from 'src/app/domain/project-exercise';
import { CRUDBaseComponent } from 'src/app/presentation/crud-base-component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ProjectExerciseUseCaseFacade } from './project-exercise-usecase.facade';

@Component({
  selector: 'srp-project-exercises',
  templateUrl: './project-exercises.component.html',
  styleUrls: ['./project-exercises.component.scss'],
  providers: [ProjectExerciseUseCaseFacade],
})
export class ProjectExercisesComponent extends CRUDBaseComponent<ProjectExercise> {
  @Input() sourse!: ProjectExercise[];
  constructor(private projectExerciseUseCaseFacade: ProjectExerciseUseCaseFacade, _srpDialogService: SrpDialogService) {
    super(_srpDialogService);
  }

  onAdd(): void {}

  onEdit(item: ProjectExercise): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new ProjectExercise(item.themeId), item), {
          dialogTitle: 'Изменить упражнение',
          primaryButtonText: 'Редактировать',
          useCase: this.projectExerciseUseCaseFacade.editUseCase,
        })
      )
      .subscribe(projectExercise => {
        this.sourse[this.sourse.findIndex(x => x.id == projectExercise.id)] = projectExercise;
      });
  }
  onDelete(projectExercise: ProjectExercise): void {
    this.dialogService
      .open(ConfirmDialogComponent, { data: projectExercise.name })
      .afterClosed.subscribe((flag: boolean) => {
        if (flag) {
          this.projectExerciseUseCaseFacade.deleteUseCase.execute(projectExercise.id).subscribe(() => {
            const index = this.sourse.findIndex(x => x.id == projectExercise.id);
            this.sourse.splice(index, 1);
          });
        }
      });
  }
}
