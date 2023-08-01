import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FilterItemContentDirective } from './filter-item-content.directive';
import { FilterItemDirective } from './filter-item.directive';
import { DynamicFilterComponent } from './filter.component';

@NgModule({
  declarations: [DynamicFilterComponent, FilterItemContentDirective, FilterItemDirective],
  exports: [DynamicFilterComponent, FilterItemContentDirective, FilterItemDirective],
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class DynamicFilterModule {}
