import { Excercise } from 'src/app/domain/index';
import { Injectable } from '@angular/core';
import { Validators } from '@angular/forms';
import {
  BaseFormElementService,
  CodeEditorControl,
  IElement,
  HorizontalPanel,
  InputControl,
  TextEditorControl,
} from 'src/app/core/form-elements';

@Injectable()
export class ExercisesFormControlService extends BaseFormElementService<Excercise> {
  elements(item: Excercise | null): IElement[] {
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
      new CodeEditorControl({
        value: item?.template,
        key: 'template',
        label: 'Шаблон',
        style: { border: '1px solid lightgrey', 'border-radius': '5px' },
        validators: [Validators.required],
      }),
      new CodeEditorControl({
        value: item?.test,
        key: 'test',
        label: 'Тест',
        style: { border: '1px solid lightgrey', 'border-radius': '5px' },
        validators: [Validators.required],
      }),
    ];
  }
}
