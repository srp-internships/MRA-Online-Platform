import { Injectable } from '@angular/core';
import { Validators } from 'ngx-editor';
import {
  BaseFormElementService,
  DropDownControl,
  IElement,
  InputControl,
  TextEditorControl,
} from 'src/app/core/form-elements';
import { DocumentArea, Documentation } from 'src/app/domain/documentation';

@Injectable()
export class DocumentationFormControlService extends BaseFormElementService<Documentation> {
  elements(item: Documentation | null): IElement[] {
    return [
      new DropDownControl<DocumentArea>({
        key: 'area',
        label: 'Для',
        value: item?.area || 0,
        options: [
          // {
          //   text: 'Admin',
          //   value: DocumentArea.Admin,
          // },
          {
            text: 'Преподаватель',
            value: DocumentArea.Teacher,
          },
          {
            text: 'Студент',
            value: DocumentArea.Student,
          },
        ],
        validators: [Validators.required],
      }),
      new InputControl({
        key: 'title',
        label: 'Заголовок',
        value: item?.title,
        validators: [Validators.required],
      }),
      new TextEditorControl({
        value: item?.content,
        key: 'content',
        label: 'Описание',
        validators: [Validators.required],
      }),
    ];
  }
}
