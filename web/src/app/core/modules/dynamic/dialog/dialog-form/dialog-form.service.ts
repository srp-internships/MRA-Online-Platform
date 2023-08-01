import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ControlBase } from 'src/app/core/form-elements';

@Injectable()
export class DynamicDialogFormService {
  toFormGroup(controls: ControlBase[]): FormGroup {
    const group: any = {};
    controls.forEach(control => {
      group[control.key] = new FormControl(getControlValue(control), control.validators);
    });
    return new FormGroup(group);

    function getControlValue(control: ControlBase) {
      if (control.type === 'date') {
        const date = Date.parse(control.value?.toString() || new Date().toString());
        return formatDate(date, 'yyyy-MM-dd', 'en-US');
      } else {
        return control.value || '';
      }
    }
  }
}
