import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { LoaderComponent } from './components/loader/loader.component';
import { SHARED_DATA } from './shared-data';
import { SafeHtmlPipe } from './safe-html.pipe';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ChangePasswordMessageComponent } from './components/change-password-message/change-password-message.component';
import { BackButtonDirective } from './back-button.directive';

const SHARED_MODULES = [CommonModule, FormsModule, ReactiveFormsModule];
const SHARED_COMPONENTS = [
  ConfirmDialogComponent,
  LoaderComponent,
  UserProfileComponent,
  SafeHtmlPipe,
  ChangePasswordComponent,
  ChangePasswordMessageComponent,
  BackButtonDirective,
];
@NgModule({
  providers: [{ provide: SHARED_DATA, useValue: {} }],
  declarations: [...SHARED_COMPONENTS],
  imports: [...SHARED_MODULES],
  exports: [...SHARED_MODULES, ...SHARED_COMPONENTS],
})
export class SharedModule {}
