import { NgModule } from '@angular/core';
import { ResetPasswordRoutes } from './reset-password.routing';
import { ByEmailComponent } from './by-email/by-email.component';
import { ApplyNewPasswordComponent } from './apply/apply-new-password.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [ByEmailComponent, ApplyNewPasswordComponent],
  imports: [SharedModule, ResetPasswordRoutes],
})
export class ResetPasswordModule {}
