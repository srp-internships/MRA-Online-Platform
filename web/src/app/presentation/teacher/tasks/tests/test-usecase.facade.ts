import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateTestUseCase } from 'src/app/data/usecases/tests/create-test.usecase';
import { DeleteTestUseCase } from 'src/app/data/usecases/tests/delete-test.usecase';
import { EditTestUseCase } from 'src/app/data/usecases/tests/edit-test.usecase';
import { EditVariantsUseCase } from 'src/app/data/usecases/tests/edit-variants.usecase';
import { GetTeacherTestsUseCase } from 'src/app/data/usecases/tests/get-teacher-tests.usecase';
import { Test } from 'src/app/domain/test';

@Injectable()
export class TestsUseCasesFacade extends BaseUseCaseFacade<Test> {
  private _getUseCase!: GetTeacherTestsUseCase;
  get getUseCase(): GetTeacherTestsUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetTeacherTestsUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateTestUseCase;
  get createUseCase(): CreateTestUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateTestUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditTestUseCase;
  public get editUseCase(): EditTestUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditTestUseCase);
    }
    return this._editUseCase;
  }

  private _editVariantUseCase!: EditVariantsUseCase;
  public get editVariantsUseCase(): EditVariantsUseCase {
    if (!this._editVariantUseCase) {
      this._editVariantUseCase = this.injector.get(EditVariantsUseCase);
    }
    return this._editVariantUseCase;
  }

  private _deleteUseCase!: DeleteTestUseCase;
  get deleteUseCase(): DeleteTestUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteTestUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
