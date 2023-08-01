import { Injectable, Injector } from '@angular/core';
import { BaseUseCaseFacade } from 'src/app/data/base/usecase-facade';
import { CreateCourseUseCase } from 'src/app/data/usecases/courses/create-course.usecase';
import { DeleteCourseUseCase } from 'src/app/data/usecases/courses/delete-course.usecase';
import { EditCourseUseCase } from 'src/app/data/usecases/courses/edit-course.usecase';
import { GetTeacherCoursesUseCase } from 'src/app/data/usecases/courses/get-teacher-courses.usecase';
import { Course } from 'src/app/domain';

@Injectable()
export class CourseUseCasesFacade extends BaseUseCaseFacade<Course> {
  private _getUseCase!: GetTeacherCoursesUseCase;
  get getUseCase(): GetTeacherCoursesUseCase {
    if (!this._getUseCase) {
      this._getUseCase = this.injector.get(GetTeacherCoursesUseCase);
    }
    return this._getUseCase;
  }

  private _createUseCase!: CreateCourseUseCase;
  get createUseCase(): CreateCourseUseCase {
    if (!this._createUseCase) {
      this._createUseCase = this.injector.get(CreateCourseUseCase);
    }
    return this._createUseCase;
  }

  private _editUseCase!: EditCourseUseCase;
  get editUseCase(): EditCourseUseCase {
    if (!this._editUseCase) {
      this._editUseCase = this.injector.get(EditCourseUseCase);
    }
    return this._editUseCase;
  }

  private _deleteUseCase!: DeleteCourseUseCase;
  get deleteUseCase(): DeleteCourseUseCase {
    if (!this._deleteUseCase) {
      this._deleteUseCase = this.injector.get(DeleteCourseUseCase);
    }
    return this._deleteUseCase;
  }

  constructor(private injector: Injector) {
    super();
  }
}
