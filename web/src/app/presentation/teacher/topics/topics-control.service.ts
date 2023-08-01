import { Injectable } from '@angular/core';
import { Validators } from '@angular/forms';
import {
  BaseFormElementService,
  IElement,
  HorizontalPanel,
  InputControl,
  TextEditorControl,
} from 'src/app/core/form-elements';

import { Topic } from 'src/app/domain';

@Injectable()
export class TopicsFormControlService extends BaseFormElementService<Topic> {
  elements(item: Topic | null): IElement[] {
    return [
      new HorizontalPanel(
        new InputControl({
          key: 'name',
          value: item?.name,
          label: 'Названия',
          validators: [Validators.required],
        }),
        new InputControl<Date>({
          key: 'startDate',
          value: item?.startDate,
          label: 'Дата начала',
          type: 'date',
          validators: [Validators.required],
        }),
        new InputControl<Date>({
          key: 'endDate',
          value: item?.endDate,
          label: 'Дата окончания',
          type: 'date',
          validators: [Validators.required],
        })
      ),
      new TextEditorControl({
        key: 'content',
        value: item?.content,
        label: 'Содержание',
        validators: [Validators.required],
      }),
    ];
  }
}
