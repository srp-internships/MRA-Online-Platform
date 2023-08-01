import { Component } from '@angular/core';
import { DialogConfig, DialogRef } from 'src/app/core/modules/srp-dialog';
import { Test } from 'src/app/domain/test';
import { FormArray, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'srp-variants-dialog',
  templateUrl: './variants-dialog.component.html',
  styleUrls: ['./variants-dialog.component.scss'],
})
export class VariantsDialogComponent {
  variantsForm!: FormGroup;
  test!: Test;

  constructor(public config: DialogConfig<Test>, public dialog: DialogRef) {
    if (config.data.variants) this.test = JSON.parse(JSON.stringify(config.data));
    this.variantsForm = new FormGroup({
      Vars: new FormArray<any>([]),
    });
    this.test.variants?.forEach(element => {
      const group = new FormGroup({
        isCorrect: new FormControl(element.isCorrect),
        value: new FormControl(element.value),
        testId: new FormControl(element.testId),
      });
      this.Vars.push(group);
    });
  }

  get Vars(): FormArray {
    return this.variantsForm.get('Vars') as FormArray;
  }

  addVar() {
    const group = new FormGroup({
      isCorrect: new FormControl(false),
      value: new FormControl(''),
      testId: new FormControl(this.test.id),
    });
    this.Vars.push(group);
  }

  removeVar(position: number) {
    if (this.Vars.controls[position].value.isCorrect) {
      throw new Error('Эту опцию нельзя удалить');
    }
    this.Vars.removeAt(position);
  }

  onSave(): void {
    this.dialog.close(this.Vars.value);
  }

  onCancel(): void {
    this.dialog.close(null);
  }

  onItemChange(position: number): void {
    this.Vars.controls.forEach(s => {
      s.value.isCorrect = false;
    });
    this.Vars.controls[position].value.isCorrect = true;
  }
}
