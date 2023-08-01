import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { GetTeacherCoursesUseCase } from 'src/app/data/usecases/courses/get-teacher-courses.usecase';
import { CreateTopicUseCase } from 'src/app/data/usecases/topic/create-topic.usecase';
import { DeleteTopicUseCase } from 'src/app/data/usecases/topic/delete-topic.usecase';
import { EditTopicUseCase } from 'src/app/data/usecases/topic/edit-topic.usecase';
import { GetTeacherTopicsUseCase } from 'src/app/data/usecases/topic/get-topics.usecase';
import { Topic } from 'src/app/domain';

@Injectable()
export class TopicUseCasesFacade extends BaseUseCaseFacade<Topic> {
  private _getUseCase!: GetTeacherTopicsUseCase;
  get getUseCase(): GetTeacherTopicsUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetTeacherTopicsUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateTopicUseCase;
  get createUseCase(): CreateTopicUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateTopicUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditTopicUseCase;
  public get editUseCase(): EditTopicUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditTopicUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteTopicUseCase;
  get deleteUseCase(): DeleteTopicUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteTopicUseCase);
    }
    return this._deleteUseCase;
  }

  private _getCoursesUseCase!: GetTeacherCoursesUseCase;
  get getCoursesUseCase(): GetTeacherCoursesUseCase {
    if (!this._getCoursesUseCase) {
      this._getCoursesUseCase = this.injector.get(GetTeacherCoursesUseCase);
    }
    return this._getCoursesUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
