import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountComponent } from './account.component';
import { AccountRoutes } from './account.routing';
import { CheckEmailMessageComponent } from './check-email-message/check-email-message.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { CallbackComponent } from "./callback/callback.component";

@NgModule({
  imports: [AccountRoutes, SharedModule],
  declarations: [AccountComponent, CheckEmailMessageComponent, SignInComponent, CallbackComponent],
})
export class AccountModule {}
