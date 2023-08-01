import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'srp-tab',
  templateUrl: './tab.component.html',
  styleUrls: ['./tab.component.scss'],
})
export class TabComponent {
  @Input() title!: string;
  @Input() active = false;
  @Output() public selected = new EventEmitter<string>();
}
