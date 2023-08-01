import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ErrorService } from '../../core/services/error.service';
import { LoaderService } from '../../shared/components/loader/loader.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}
  handleError(error: Error | HttpErrorResponse | any): void {
    const chunkFailedMessage = /Loading chunk [\d]+ failed/;
    if (chunkFailedMessage.test(error.message)) {
      if (confirm('Доступна новая версия. Загрузить новую версию?')) {
        window.location.reload();
      }
    }
    const toastr = this.injector.get(ToastrService);
    const errorService = this.injector.get(ErrorService);
    const zone = this.injector.get(NgZone);
    const loader = this.injector.get(LoaderService);
    loader.hide();
    //const logger = this.injector.get(LoggingService);
    if (error['rejection']) {
      error = error['rejection'];
    }

    const isHttpError = error instanceof HttpErrorResponse;
    const err = isHttpError ? errorService.getServerError(error) : errorService.getClientError(error);
    zone.run(() => {
      toastr.error(err.message, isHttpError ? `Ошибка HTTP: ${error.status}` : 'Ошибка');
    });
    // Always log errors
    //logger.logError(err.message, err.stack || '');
    console.error(error);
  }
}
