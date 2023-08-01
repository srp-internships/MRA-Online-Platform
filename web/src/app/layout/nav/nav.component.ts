import { Component } from '@angular/core';
import { AccountRouterLinks, AccountRoutingModule } from 'src/app/core/routings';
import { UserProfileComponent } from 'src/app/shared/components/user-profile/user-profile.component';
import { LayoutService } from '../layout.service';

@Component({
  selector: 'srp-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss'],
})
export class NavComponent {
  links = AccountRouterLinks;
  constructor(public layout: LayoutService, public accountRoutingModule: AccountRoutingModule) {}

  onProfileClick() {
    this.layout.popupProfileInfo(UserProfileComponent);
  }

  onToggle() {
    const currentValue = this.layout.toggleMenu.value;
    this.layout.onToggleMenu(!currentValue);
  }
}
