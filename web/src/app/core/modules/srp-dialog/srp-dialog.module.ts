import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SrpDialogComponent } from './srp-dialog.component';
import { SrpDialogDirective } from './dialog.directive';

@NgModule({
  declarations: [SrpDialogComponent, SrpDialogDirective],
  imports: [CommonModule],
  entryComponents: [SrpDialogComponent],
  exports: [SrpDialogComponent],
})
export class SrpDialogModule {}
