import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class PrintElementService {
  print(element: HTMLElement, title: string): void {
    var a = window.open('/print.html')!;
    a.addEventListener('DOMContentLoaded', () => {
      a.document.title = title;
      a.document.body.innerHTML += `<div class="title has-text-centered">${title}</div> <hr/>`;
      a.document.body.innerHTML += element.outerHTML;
      a.print();
      setTimeout(() => {
        a!.close();
      }, 300);
    });
  }
}
