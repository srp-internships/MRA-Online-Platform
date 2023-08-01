import { NgModule } from '@angular/core';
import { AdminComponent } from './admin.component';
import { TeachersComponent } from './teachers/teachers.component';
import { DynamicFilterModule } from 'src/app/core/modules/dynamic/filter';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminRoutes } from './admin.routing';
import { DynamicDialogModule } from 'src/app/core/modules/dynamic/dialog';
import { DocumentationComponent } from './documentation/documentation.component';

@NgModule({
  imports: [SharedModule, AdminRoutes, DynamicFilterModule, DynamicDialogModule],
  declarations: [AdminComponent, TeachersComponent, DocumentationComponent],
})
export class AdminModule {}
