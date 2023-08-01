import { Component } from '@angular/core';
import { SrpDialogService } from 'src/app/core/modules/srp-dialog';
import { BetaComponent } from '../common-pages/beta/beta.component';

@Component({
  selector: 'srp-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
})
export class AccountComponent {
  constructor(private readonly _dialogService: SrpDialogService) {}

  onBetaClick() {
    this._dialogService.open(BetaComponent);
  }
}
