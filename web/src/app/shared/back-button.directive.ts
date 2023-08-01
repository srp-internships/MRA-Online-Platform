import { Directive, HostListener } from '@angular/core';
import { NavigationService } from '../core/routings';

@Directive({
  selector: '[srpBackButton]',
})
export class BackButtonDirective {
  constructor(private readonly navigation: NavigationService) {}

  @HostListener('click')
  onClick(): void {
    this.navigation.back();
  }
}
