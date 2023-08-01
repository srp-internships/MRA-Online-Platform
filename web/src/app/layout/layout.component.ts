import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AccountRouterLinks, AccountRoutingModule } from '../core/routings';
import { UserProfileComponent } from '../shared/components/user-profile/user-profile.component';
import { LayoutService } from './layout.service';

@Component({
  selector: 'srp-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
})
export class LayoutComponent implements OnInit {
  links = AccountRouterLinks;
  constructor(public layout: LayoutService, public accountRoutingModule: AccountRoutingModule) {}

  @ViewChild('main') private main!: ElementRef;
  onProfileClick() {
    this.layout.popupProfileInfo(UserProfileComponent);
  }

  ngOnInit(): void {
    this.layout.scrollToTop$.subscribe(() => {
      this.main.nativeElement.scrollTo(0, 0);
    });
  }
}
