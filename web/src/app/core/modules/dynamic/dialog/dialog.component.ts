import { Component, ErrorHandler, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { BaseFormElementService, IElement, FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { BaseEntity } from 'src/app/domain';
import { DialogConfig, DialogRef } from '../../srp-dialog';
import { DynamicDialogData } from './dialog-data';
import { DynamicDialogFormService } from './dialog-form/dialog-form.service';

@Component({
  selector: 'srp-dynamic-dialog',
  templateUrl: './dialog.component.html',
  providers: [
    DynamicDialogFormService,
    {
      provide: BaseFormElementService,
      useFactory: (model: FormStrategyModel) => model.model,
      deps: [FORM_STRATEGY],
    },
  ],
  styleUrls: ['./dialog.component.scss'],
})
export class DynamicDialogComponent implements OnInit {
  form!: FormGroup;
  elements!: IElement[];
  dialogData: DynamicDialogData;
  constructor(
    public config: DialogConfig<DynamicDialogData>,
    public dialog: DialogRef,
    private formStrategy: BaseFormElementService,
    private dynamicFormService: DynamicDialogFormService,
    private errorHandler: ErrorHandler
  ) {
    this.dialogData = config.data;
  }
  ngOnInit(): void {
    try {
      const elements = this.formStrategy.elements(this.dialogData.entity);
      if (elements instanceof Observable<Element[]>) {
        elements.subscribe(c => {
          this.elements = c;
          this.form = this.dynamicFormService.toFormGroup(this.elements.selectMany(x => x.controls()));
        });
      } else {
        this.elements = elements;
        this.form = this.dynamicFormService.toFormGroup(this.elements.selectMany(x => x.controls()));
      }
    } catch (error) {
      this.errorHandler.handleError(error);
      this.dialog.close(null);
    }
  }

  onSubmit(event: Event) {
    event.preventDefault();

    if (this.form.invalid) {
      return;
    }

    const newData: BaseEntity = Object.assign(this.dialogData.entity, this.form.value);

    if (this.dialogData.useCase) {
      this.dialogData.useCase.execute(newData).subscribe(response => {
        if (typeof response === 'string') {
          newData.id = response;
        }
        this.dialog.close(newData);
      });
    } else {
      this.dialog.close(newData);
    }
  }

  onClose(event: Event) {
    event.preventDefault();
    this.dialog.close(null);
  }
}
