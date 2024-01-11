import { NgModule } from '@angular/core';
import { DynamicFilterModule } from 'src/app/core/modules/dynamic/filter';
import { SharedModule } from 'src/app/shared/shared.module';
import { ComingSoonRoutes } from './ComingSoon.Routing';
import { DynamicDialogModule } from 'src/app/core/modules/dynamic/dialog';
import {ComingSoon} from "./ComingSoon";

@NgModule({
  imports: [SharedModule, ComingSoonRoutes, DynamicFilterModule, DynamicDialogModule],
  declarations: [ComingSoon],
})
export class ComingSoonModule {}
