import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { RouterNavigation } from '../routings';

@Injectable({ providedIn: 'root' })
export class CustomRxJsOperators {
  constructor(private toastr: ToastrService) {}

  navigateIfEmpty(router: RouterNavigation, message: string) {
    const self = this;
    return function <T>(source: Observable<T[]>): Observable<T[]> {
      return new Observable(subscriber => {
        const subscription = source.subscribe({
          next(value) {
            if (!value.length) {
              router.navigate();
              self.toastr.info(message);
              subscriber.complete();
            }
            {
              subscriber.next(value);
            }
          },
          error(error) {
            subscriber.error(error);
          },
          complete() {
            subscriber.complete();
          },
        });

        return () => subscription.unsubscribe();
      });
    };
  }
}
