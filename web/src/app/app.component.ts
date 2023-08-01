import { Component } from '@angular/core';

Array.prototype.selectMany = function (cl): any[] {
  return this.map(cl).reduce((a, b) => a.concat(b), []);
};

Date.prototype.getDayDiff = function (srartDate: Date): number {
  const msInDay = 24 * 60 * 60 * 1000;

  return Math.round(Math.abs(Number(this) - Number(srartDate)) / msInDay);
};

declare global {
  interface Array<T> {
    selectMany<R>(o: (item: T) => R[]): Array<R>;
  }

  interface Date {
    getDayDiff: (srartDate: Date) => number;
  }
}
@Component({
  selector: 'srp-root',
  templateUrl: './app.component.html',
})
export class AppComponent {}
