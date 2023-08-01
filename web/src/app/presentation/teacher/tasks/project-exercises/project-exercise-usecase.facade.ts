import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateProjectExerciseUseCase } from 'src/app/data/usecases/projectExercise/create-projectExercise.usecase';
import { DeleteProjectExeriseUseCase } from 'src/app/data/usecases/projectExercise/delete-projectExerise.usecase';
import { EditProjectExerciseUseCase } from 'src/app/data/usecases/projectExercise/edit-projectExerise.usecase';
import { GetProjectExercisesUseCase } from 'src/app/data/usecases/projectExercise/get-projectExerise.usecase';
import { ProjectExercise } from 'src/app/domain/project-exercise';

@Injectable()
export class ProjectExerciseUseCaseFacade extends BaseUseCaseFacade<ProjectExercise> {
  private _getUseCase!: GetProjectExercisesUseCase;

  get getUseCase(): GetProjectExercisesUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetProjectExercisesUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateProjectExerciseUseCase;
  get createUseCase(): CreateProjectExerciseUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateProjectExerciseUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditProjectExerciseUseCase;
  public get editUseCase(): EditProjectExerciseUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditProjectExerciseUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteProjectExeriseUseCase;
  get deleteUseCase(): DeleteProjectExeriseUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteProjectExeriseUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
