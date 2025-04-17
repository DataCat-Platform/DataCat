import {
  Component,
  EventEmitter,
  Output,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { FormsModule } from '@angular/forms';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { PopoverModule } from 'primeng/popover';
import { DateFilterOperand, Filter, FilterOperandKind } from './filtering-menu.types';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'filtering-menu',
  standalone: true,
  imports: [
    CommonModule,
    InputGroupModule,
    InputGroupAddonModule,
    SelectModule,
    ButtonModule,
    TooltipModule,
    PopoverModule,
    FormsModule,
  ],
  templateUrl: './filtering-menu.component.html',
  styleUrl: './filtering-menu.component.scss',
})
export class FilterComponent {
  filters: Filter[] = [];

  @Output() onFiltersChange: EventEmitter<Filter[]> = new EventEmitter<
    Filter[]
  >();

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
          operand: new DateFilterOperand(),
        },
      },
    ];
    this.notifyFiltersChanged();
  }

  removeFilter(index: number) {
    this.filters = this.filters.filter((_, idx) => idx !== index);
    this.notifyFiltersChanged();
  }

  clearFilters() {
    this.filters = [];
    this.notifyFiltersChanged();
  }

  getOperandComponent(filter: Filter) {
    switch (filter.operation.operand.type) {
      case FilterOperandKind.DATE: {
        return null;
      }
      default: {
        return null;
      }
    }
  }
}
