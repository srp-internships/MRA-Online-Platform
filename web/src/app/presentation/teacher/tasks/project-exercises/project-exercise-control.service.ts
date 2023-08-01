import { Injectable } from '@angular/core';
import { Validators } from '@angular/forms';
import {
  BaseFormElementService,
  IElement,
  HorizontalPanel,
  InputControl,
  TextEditorControl,
} from 'src/app/core/form-elements';
import { ProjectExercise } from 'src/app/domain/project-exercise';

@Injectable()
export class ProjectExerciseFormControlService extends BaseFormElementService<ProjectExercise> {
  elements(item: ProjectExercise | null): IElement[] {
    return [
      new HorizontalPanel(
        new InputControl({
          value: item?.name,
          key: 'name',
          label: 'Названия',
          validators: [Validators.required],
        }),
        new InputControl<number>({
          value: item?.rating,
          key: 'rating',
          label: 'Рейтинг',
          type: 'number',
          validators: [Validators.required, Validators.pattern(/^([0-9]|10)$/)],
        })
      ),
      new TextEditorControl({
        value: item?.description,
        key: 'description',
        label: 'Описание',
        validators: [Validators.required],
      }),
    ];
  }
}
