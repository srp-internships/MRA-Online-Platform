import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateDocumentationUseCase } from 'src/app/data/usecases/admin/documentationCRUD/create-documentation.usecase';
import { DeleteDocumentationUseCase } from 'src/app/data/usecases/admin/documentationCRUD/delete-documentation.usecase';
import { EditDocumentationUseCase } from 'src/app/data/usecases/admin/documentationCRUD/edit-documentation.usecase';
import { GetDocumentationsUseCase } from 'src/app/data/usecases/admin/documentationCRUD/get-documentation.usecase';
import { Documentation } from 'src/app/domain/documentation';

@Injectable()
export class DocumentationsUseCasesFacade extends BaseUseCaseFacade<Documentation> {
  private _getUseCase!: GetDocumentationsUseCase;
  get getUseCase(): GetDocumentationsUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetDocumentationsUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateDocumentationUseCase;
  get createUseCase(): CreateDocumentationUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateDocumentationUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditDocumentationUseCase;
  get editUseCase(): EditDocumentationUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditDocumentationUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteDocumentationUseCase;
  get deleteUseCase(): DeleteDocumentationUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteDocumentationUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
