import { Component, Inject, OnInit } from '@angular/core';
import { PagingService } from 'src/app/core/pagings';
import { NavigationService } from 'src/app/core/routings';
import { HELPER_SERVICE, HelperService } from 'src/app/data/base/helper-service';
import { Documentation } from 'src/app/domain/documentation';
import { LayoutService } from 'src/app/layout/layout.service';

@Component({
  selector: 'srp-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss'],
  providers: [PagingService],
})
export class HelpComponent implements OnInit {
  doc?: Documentation;
  readonly returnUrl: string = '';
  constructor(
    @Inject(HELPER_SERVICE) private readonly _helperService: HelperService,
    public readonly _pagingService: PagingService,
    private readonly _layout: LayoutService,
    private _navigationService: NavigationService
  ) {
    _layout.clearSideNav();
  }

  ngOnInit(): void {
    this._helperService.help().subscribe(doc => {
      this.doc = Object.assign(new Documentation(), doc);
      this._pagingService.build(this.doc);
    });
  }

  onPaginationClick(n: number) {
    this.doc!.content = this._pagingService.content(n);
    this._layout.scrollTop();
  }
}
