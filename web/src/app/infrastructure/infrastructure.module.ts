import { APP_INITIALIZER, ErrorHandler, NgModule } from '@angular/core';
import { GlobalErrorHandler } from '../infrastructure/error-handling/global-error-handler.service';
import { tokenInterceptorProvider } from '../infrastructure/interceptors/token-interceptor';
import { errorInterceptorProvider } from '../infrastructure/interceptors/error-interceptor';
import { CommonModule } from '@angular/common';

export function initializerFactory() {
  return (): Promise<any> => {
    return Promise.resolve(true);
  };
}

@NgModule({
  imports: [CommonModule],
  providers: [
    {
      provide: ErrorHandler,
      useClass: GlobalErrorHandler,
    },
    tokenInterceptorProvider,
    errorInterceptorProvider,
    {
      provide: APP_INITIALIZER,
      useFactory: initializerFactory,
      deps: [],
      multi: true,
    },
  ],
})
export class InfrastructureModule {}
