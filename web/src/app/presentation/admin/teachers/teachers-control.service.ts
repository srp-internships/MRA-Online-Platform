import { Injectable } from '@angular/core';
import { Validators } from '@angular/forms';
import { BaseFormElementService, IElement, HorizontalPanel, InputControl } from 'src/app/core/form-elements';
import { Teacher } from 'src/app/domain/teacher';
import { CustomValidationService } from 'src/app/core/services/custom-validation.service';

@Injectable()
export class TeachersFormControlService extends BaseFormElementService<Teacher> {
  elements(item: Teacher | null): IElement[] {
    return [
      new HorizontalPanel(
        new InputControl({
          key: 'firstName',
          label: 'Имя',
          value: item?.firstName,
          validators: [Validators.required],
        }),
        new InputControl({
          key: 'lastName',
          label: 'Фамилия',
          value: item?.lastName,
          validators: [Validators.required],
        })
      ),
      new HorizontalPanel(
        new InputControl({
          key: 'phoneNumber',
          label: 'Номер телефона',
          value: item?.phoneNumber,
          validators: [Validators.required],
        }),
        new InputControl<Date>({
          key: 'dateOfBirth',
          label: 'Дата рождения',
          value: item?.dateOfBirth,
          type: 'date',
          validators: [Validators.required],
        })
      ),
      new InputControl({
        key: 'address',
        label: 'Адрес',
        value: item?.address,
        validators: [Validators.required],
      }),
      new HorizontalPanel(
        new InputControl({
          key: 'country',
          label: 'Страна',
          value: item?.country,
          validators: [Validators.required],
        }),
        new InputControl({
          key: 'region',
          label: 'Область, край',
          value: item?.region,
          validators: [Validators.required],
        }),
        new InputControl({
          key: 'city',
          label: 'Город',
          value: item?.city,
          validators: [Validators.required],
        })
      ),
    ];
  }
}

@Injectable()
export class TeachersPostFormControlService extends BaseFormElementService<Teacher> {
  constructor(private customValidator: CustomValidationService) {
    super();
  }

  elements(item: Teacher | null): IElement[] {
    return [
      new InputControl({
        key: 'email',
        label: 'Эл. адрес',
        type: 'email',
        value: item?.email,
        validators: [Validators.required, Validators.email],
      }),
      new InputControl({
        key: 'password',
        label: 'Пароль',
        type: 'password',
        value: item?.password,
        validators: [Validators.required, this.customValidator.passwordValidator()],
      }),
    ];
  }
}
