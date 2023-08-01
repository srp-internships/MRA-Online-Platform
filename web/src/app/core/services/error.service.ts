import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

export interface SrpError {
  message: string;
  stack?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ErrorService {
  getClientError(error: Error): SrpError {
    const message = error.message ? error.message : error.toString();
    const stack = error.stack;
    return { message, stack };
  }

  getServerError(error: HttpErrorResponse): SrpError {
    if (!navigator.onLine) {
      return { message: 'Нет соединения с интернетом' };
    }

    if (error.status === 0) {
      return { message: 'Не удалось установить соединение с сервером' };
    }

    if (error.status === 500) {
      return { message: 'Что-то пошло не так на сервере.' };
    }

    const message = error.error?.errors
      ? error.error?.errors.map((x: any) => x.errorMessage)
      : error.error || error.message;
    return { message, stack: error.toString() };
  }
}
