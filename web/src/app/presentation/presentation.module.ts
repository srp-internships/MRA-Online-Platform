import { NgModule } from '@angular/core';
import { PresentationComponent } from './presentation.component';
import { PresentationRoutes } from './presentation.routing';
import { LayoutModule } from '../layout/layout.module';
import { SharedModule } from '../shared/shared.module';
import { HelpComponent } from './common-pages/help/help.component';

@NgModule({
  imports: [SharedModule, PresentationRoutes, LayoutModule],
  declarations: [PresentationComponent, HelpComponent],
})
export class PresentationModule {}
