import { Component, EventEmitter, Output } from '@angular/core';
import { LayoutService } from '../layout.service';

export interface SideNav {
  title?: string;
  items: SideNavItem[];
}

export interface SideNavItem {
  link: string | any[];
  text: string;
  disabled: boolean;
  disabledText?: string;
  params?: any;
  icon?: string;
}

@Component({
  selector: 'srp-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss'],
})
export class SideNavComponent {
  @Output() clickItem: EventEmitter<SideNavItem> = new EventEmitter<SideNavItem>();
  constructor(public layout: LayoutService) {}
}
