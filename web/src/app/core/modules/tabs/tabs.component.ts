import { AfterContentInit, Component, ContentChildren, ElementRef, Input, QueryList, ViewChild } from '@angular/core';
import { TabComponent } from './tab/tab.component';

@Component({
  selector: 'srp-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.scss'],
})
export class TabsComponent implements AfterContentInit {
  @Input() classes?: string[];
  @ContentChildren(TabComponent) tabs!: QueryList<TabComponent>;
  @ViewChild('tabsRef', { static: true }) tabsRef!: ElementRef<HTMLDivElement>;

  ngAfterContentInit(): void {
    if (this.classes) {
      this.tabsRef.nativeElement.classList.add(...this.classes);
    }
    const activeTabs = this.tabs.find(tab => tab.active) as TabComponent;

    if (!activeTabs) {
      this.selectTab(this.tabs.first);
    } else {
      activeTabs.selected.emit(activeTabs.title);
    }
  }

  selectTab(tab: TabComponent) {
    this.tabs.toArray().forEach(tab => (tab.active = false));
    tab.active = true;
    tab.selected.emit(tab.title);
  }
}
