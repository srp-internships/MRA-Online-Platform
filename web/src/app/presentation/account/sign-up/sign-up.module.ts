import { NgModule } from '@angular/core';
import { RecaptchaModule, RecaptchaFormsModule, RECAPTCHA_SETTINGS, RecaptchaSettings } from 'ng-recaptcha';
import { SharedModule } from 'src/app/shared/shared.module';
import { environment } from 'src/environments/environment';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { SignUpComponent } from './sign-up.component';
import { SignUpRoutes } from './sign-up.routing';

@NgModule({
  declarations: [SignUpComponent, ConfirmEmailComponent],
  imports: [SharedModule, SignUpRoutes, RecaptchaModule, RecaptchaFormsModule],
  providers: [
    {
      provide: RECAPTCHA_SETTINGS,
      useValue: {
        siteKey: environment.recaptcha.siteKey,
      } as RecaptchaSettings,
    },
  ],
})
export class SignUpModule {}
