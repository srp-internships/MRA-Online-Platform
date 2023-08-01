import { Inject, Injectable, Type } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ODataService, ODATA_SERVICE } from '../data/base/odata-request';
import { UserProfile } from '../domain';
import { TokenManagerService } from '../core/services/token-storage.service';
import { SideNav } from './side-nav/side-nav.component';
import { SrpDialogService } from '../core/modules/srp-dialog';
import { BetaComponent } from '../presentation/common-pages/beta/beta.component';

@Injectable()
export class LayoutService implements IScrollTop {
  private readonly _scrollToTop$: Subject<void> = new Subject<void>();
  get scrollToTop$(): Observable<void> {
    return this._scrollToTop$;
  }
  get email(): string {
    return this.tokenManager.getEmail();
  }

  get role(): string {
    return this.tokenManager.getRole();
  }
  constructor(
    @Inject(ODATA_SERVICE) private odataService: ODataService,
    private dialogService: SrpDialogService,
    private tokenManager: TokenManagerService
  ) {}

  toggleMenu: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private _sideNav: SideNav[] = [];
  public get sideNavs(): SideNav[] {
    return this._sideNav;
  }

  setSideNavItems(items: SideNav[]): void {
    this._sideNav = items;
  }

  clearSideNav() {
    this._sideNav = [];
  }

  onToggleMenu(flag: boolean) {
    this.toggleMenu.next(flag);
  }

  popupProfileInfo<T>(component: Type<T>) {
    this.odataService
      .executeQuery('$select=FirstName,LastName,Birthdate,Contact&$expand=Contact($select=PhoneNumber)')
      .subscribe((data: UserProfile) => {
        this.dialogService.open(component, { data });
      });
  }

  scrollTop() {
    this._scrollToTop$.next();
  }

  openBetaDialog() {
    this.dialogService.open(BetaComponent);
  }
}

export interface IScrollTop {
  scrollTop: () => void;
}
