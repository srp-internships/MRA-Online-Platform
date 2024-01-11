import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { ToastrModule } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { JwtModule } from '@auth0/angular-jwt';
import { NotFoundPageComponent } from './static-pages/not-found-page/not-found-page.component';
import { ProtectedPageComponent } from './static-pages/protected-page/protected-page.component';
import { DataModulte } from './data/data.module';
import { InfrastructureModule } from './infrastructure/infrastructure.module';
import { FORM_STRATEGY } from './core/form-elements';
import { SrpDialogModule } from './core/modules/srp-dialog/srp-dialog.module';
import { BetaComponent } from './presentation/common-pages/beta/beta.component';
import {ComingSoonInterceptor} from "./injectors";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";

export function tokenGetter() {
  return localStorage.getItem('access_token');
}

@NgModule({
  declarations: [AppComponent, NotFoundPageComponent, ProtectedPageComponent, BetaComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    DataModulte,
    InfrastructureModule,
    SrpDialogModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        allowedDomains: [environment.baseUrl],
        disallowedRoutes: [],
      },
    }),
    ToastrModule.forRoot({ positionClass: 'toast-bottom-right', preventDuplicates: true }),
  ],
  bootstrap: [AppComponent],
  providers: [{ provide: FORM_STRATEGY, useValue: {} },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ComingSoonInterceptor,
      multi: true,
    },],
})
export class AppModule {}
