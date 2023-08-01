import { Component, Inject, OnInit } from '@angular/core';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { CRUDBaseComponent } from '../../crud-base-component';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { FormStrategyModel, FORM_STRATEGY } from 'src/app/core/form-elements';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';
import { Documentation } from 'src/app/domain/documentation';
import { DocumentationsUseCasesFacade } from './documentation-usecase.facade';
import { DocumentationFormControlService } from './documentation-control.service';

@Component({
  selector: 'srp-documentation',
  templateUrl: './documentation.component.html',
  providers: [DocumentationsUseCasesFacade, DocumentationFormControlService],
  styleUrls: ['./documentation.component.scss'],
})
export class DocumentationComponent extends CRUDBaseComponent<Documentation> implements OnInit {
  constructor(
    _dialog: SrpDialogService,
    private docUseCasesFacade: DocumentationsUseCasesFacade,
    private loader: LoaderService,
    private postFormControlService: DocumentationFormControlService,
    @Inject(FORM_STRATEGY) private formStrategy: FormStrategyModel
  ) {
    super(_dialog);
    this.formStrategy.model = this.postFormControlService;
  }
  ngOnInit(): void {
    this.loader.show();
    this.docUseCasesFacade.getUseCase.execute().subscribe(docs => {
      this.items = docs;
      this.loader.hide();
    });
  }

  onAdd(): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Documentation()), {
          dialogTitle: 'Новый документ',
          useCase: this.docUseCasesFacade.createUseCase,
          primaryButtonText: 'Сохранить',
        })
      )
      .subscribe(doc => {
        this.items.push(doc);
      });
  }
  onEdit(item: Documentation): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Documentation(), item), {
          dialogTitle: 'Изменить документ',
          useCase: this.docUseCasesFacade.editUseCase,
          primaryButtonText: 'Редактировать',
        })
      )
      .subscribe(doc => {
        this.items[this.items.findIndex(x => x.id == doc.id)] = doc;
      });
  }
  onDelete(item: Documentation): void {
    this.dialogService.open(ConfirmDialogComponent, { data: item.title }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this.docUseCasesFacade.deleteUseCase.execute(item.id).subscribe(() => {
          const index = this.items.findIndex(x => x.id == item.id);
          this.items.splice(index, 1);
        });
      }
    });
  }
}
