import { Inject, Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { List } from 'src/app/domain/list';
import { SharedData, SHARED_DATA } from 'src/app/shared/shared-data';

@Injectable()
export class ResolvedContentTitle implements Resolve<string> {
  constructor(@Inject(SHARED_DATA) private _sharedData: SharedData) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): string | Observable<string> | Promise<string> {
    const topicId = route.paramMap.get('topicId')!;
    const topics = this._sharedData['topicList'] as List[];
    return topics?.find(x => x.id === topicId)?.name ?? 'Guide page';
  }
}
