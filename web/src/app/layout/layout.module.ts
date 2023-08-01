import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { NavComponent } from './nav/nav.component';
import { SideNavComponent } from './side-nav/side-nav.component';

@NgModule({
  declarations: [LayoutComponent, NavComponent, SideNavComponent],
  exports: [LayoutComponent, NavComponent, SideNavComponent],
  imports: [RouterModule, CommonModule],
})
export class LayoutModule {}
