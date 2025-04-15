import { Component, EventEmitter, Input, Output } from '@angular/core';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { TooltipModule } from 'primeng/tooltip';
import { PopoverModule } from 'primeng/popover';

@Component({
  selector: 'filters',
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
  filters!: any[];

  @Input() availableFilters!: any[];

  @Output() onFiltersChange!: EventEmitter<any[]>;

  notifyFiltersChanged() {
    this.onFiltersChange.emit(this.filters);
  }
}
