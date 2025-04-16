import { Component, EventEmitter, Input, Output } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { PopoverModule } from 'primeng/popover';
import { Filter } from './filters-panel.types';

@Component({
  selector: 'filter-panel',
  standalone: true,
  imports: [
    InputGroupModule,
    InputGroupAddonModule,
    SelectModule,
    ButtonModule,
    TooltipModule,
    PopoverModule,
  ],
  templateUrl: './filters-panel.component.html',
})
export class FilterComponent {
  filters: Filter[] = [];

  @Output() onFiltersChange: EventEmitter<Filter[]> = new EventEmitter<Filter[]>();

  notifyFiltersChanged() {
    this.onFiltersChange.emit(this.filters);
  }

  addFilter() {
    this.filters = [
      ...this.filters,
      {
        prefix: 'Something',
        operation: {
          variants: ['is', 'is not'],
          selectedVariant: 'is',
          operand: {},
        },
      }
    ];
    this.notifyFiltersChanged();
  }
}
