import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateTeacherUseCase } from 'src/app/data/usecases/admin/create-teacher.usecase';
import { DeleteTeacherUseCase } from 'src/app/data/usecases/admin/delete-teacher.usecase';
import { EditTeacherUseCase } from 'src/app/data/usecases/admin/edit-teacher.usecase';
import { GetTeachersUseCase } from 'src/app/data/usecases/admin/get-teacher.usecase';
import { Teacher } from 'src/app/domain/teacher';

@Injectable()
export class TeachersUseCasesFacade extends BaseUseCaseFacade<Teacher> {
  private _getUseCase!: GetTeachersUseCase;
  get getUseCase(): GetTeachersUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetTeachersUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateTeacherUseCase;
  get createUseCase(): CreateTeacherUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateTeacherUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditTeacherUseCase;
  get editUseCase(): EditTeacherUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditTeacherUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteTeacherUseCase;
  get deleteUseCase(): DeleteTeacherUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteTeacherUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
