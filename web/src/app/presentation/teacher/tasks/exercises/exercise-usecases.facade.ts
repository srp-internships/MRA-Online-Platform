import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateExerciseUseCase } from 'src/app/data/usecases/exercises/create-exercise.usecase';
import { DeleteExerciseUseCase } from 'src/app/data/usecases/exercises/delete-exercise.usecase';
import { EditExerciseUseCase } from 'src/app/data/usecases/exercises/edit-exercise.usecase';
import { GetTeacherExercisesUseCase } from 'src/app/data/usecases/exercises/get-teacher-exercises.usecase';
import { Excercise } from 'src/app/domain';

@Injectable()
export class ExerciseUseCasesFacade extends BaseUseCaseFacade<Excercise> {
  private _getUseCase!: GetTeacherExercisesUseCase;
  get getUseCase(): GetTeacherExercisesUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetTeacherExercisesUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateExerciseUseCase;
  get createUseCase(): CreateExerciseUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateExerciseUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditExerciseUseCase;
  public get editUseCase(): EditExerciseUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditExerciseUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteExerciseUseCase;
  get deleteUseCase(): DeleteExerciseUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteExerciseUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
