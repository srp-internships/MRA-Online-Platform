import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BackgroundTaskService {
  tasks$?: Promise<any[]>;

  addTasks(...tasks: Promise<any>[]) {
    this.tasks$ = Promise.all(tasks);
  }
}
