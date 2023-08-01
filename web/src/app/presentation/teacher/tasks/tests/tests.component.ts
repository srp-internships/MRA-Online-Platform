import { Component, Input } from '@angular/core';
import { first, filter } from 'rxjs/operators';
import { DynamicDialogData } from 'src/app/core/modules/dynamic/dialog';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { Test } from 'src/app/domain/test';
import { Variant } from 'src/app/domain/variant';
import { CRUDBaseComponent } from 'src/app/presentation/crud-base-component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { TestsUseCasesFacade } from './test-usecase.facade';
import { VariantsDialogComponent } from './variants-dialog/variants-dialog.component';

@Component({
  selector: 'srp-tests',
  templateUrl: './tests.component.html',
  styleUrls: ['./tests.component.scss'],
  providers: [TestsUseCasesFacade],
})
export class TestsComponent extends CRUDBaseComponent<Test> {
  @Input() sourse!: Test[];
  constructor(private testUseCasesFacade: TestsUseCasesFacade, private _srpDialogService: SrpDialogService) {
    super(_srpDialogService);
  }

  onAdd(): void {}

  onEdit(item: Test): void {
    super
      .openDynamicDailog(
        new DynamicDialogData(Object.assign(new Test(item.themeId), item), {
          dialogTitle: 'Изменить упражнение',
          primaryButtonText: 'Редактировать',
          useCase: this.testUseCasesFacade.editUseCase,
        })
      )
      .subscribe(test => {
        this.sourse[this.sourse.findIndex(x => x.id == test.id)] = test;
      });
  }
  onDelete(test: Test): void {
    this.dialogService.open(ConfirmDialogComponent, { data: test.name }).afterClosed.subscribe((flag: boolean) => {
      if (flag) {
        this.testUseCasesFacade.deleteUseCase.execute(test.id).subscribe(() => {
          const index = this.sourse.findIndex(x => x.id == test.id);
          this.sourse.splice(index, 1);
        });
      }
    });
  }
  onOpenVariants(index: number) {
    this._srpDialogService
      .open<Test>(VariantsDialogComponent, { data: this.sourse[index] })
      .afterClosed.pipe(
        filter(i => !!i),
        first()
      )
      .subscribe((variants: Variant[]) => {
        this.testUseCasesFacade.editVariantsUseCase.execute(variants).subscribe(() => {
          this.sourse[index].variants = variants;
        });
      });
  }
}
