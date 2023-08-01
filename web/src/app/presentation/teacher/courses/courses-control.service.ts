import { Course } from 'src/app/domain/index';
import { Injectable } from '@angular/core';
import { Validators } from '@angular/forms';
import { BaseFormElementService, IElement, InputControl } from 'src/app/core/form-elements';

@Injectable()
export class CoursesFormControlService extends BaseFormElementService<Course> {
  elements(item: Course | null): IElement[] {
    return [
      new InputControl({
        key: 'name',
        label: 'Названия',
        value: item?.name,
        validators: [Validators.required],
      }),
      new InputControl({
        key: 'courseLanguage',
        label: 'Язык курсы',
        value: item?.courseLanguage,
        validators: [Validators.required],
      }),
    ];
  }
}
