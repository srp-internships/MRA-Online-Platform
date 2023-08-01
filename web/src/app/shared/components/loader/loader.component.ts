import { Component, Input, TemplateRef } from '@angular/core';
import { LoaderService } from 'src/app/shared/components/loader/loader.service';

@Component({
  selector: 'srp-loader',
  templateUrl: './loader.component.html',
})
export class LoaderComponent {
  @Input() templateRef!: TemplateRef<any>;
  constructor(public loaderService: LoaderService) {}
}
