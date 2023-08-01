import { filter, Observable } from 'rxjs';
import { DynamicDialogComponent, DynamicDialogData } from '../core/modules/dynamic/dialog';
import { SrpDialogService } from '../core/modules/srp-dialog';
import { BaseEntity } from '../domain/base-entity';

export abstract class CRUDBaseComponent<T extends BaseEntity> {
  items!: T[];
  constructor(protected dialogService: SrpDialogService) {}

  abstract onAdd(): void;
  abstract onEdit(item: T): void;
  abstract onDelete(course: T): void;

  openDynamicDailog(data: DynamicDialogData): Observable<T> {
    return this.dialogService
      .open<DynamicDialogData>(DynamicDialogComponent, { data })
      .afterClosed.pipe(filter(x => x));
  }
}
