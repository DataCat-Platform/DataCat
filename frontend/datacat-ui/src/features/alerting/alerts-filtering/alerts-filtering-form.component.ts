import { Component, EventEmitter, Output } from '@angular/core';
import { AlertsFilter } from '../../../entities/alerting/alerts-filter.types';
import { PanelModule } from 'primeng/panel';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SelectModule } from 'primeng/select';
import { ChipModule } from 'primeng/chip';
import { AlertStatus, DataSource } from '../../../entities/alerting';
import { InputTextModule } from 'primeng/inputtext';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { ButtonModule } from 'primeng/button';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { FieldsetModule } from 'primeng/fieldset';
import { from } from 'rxjs';
import { FAKE_DATASOURCE } from '../../../shared/mock/fakes';

@Component({
  standalone: true,
  selector: 'datacat-alerts-filtering-form',
  templateUrl: './alerts-filtering-form.component.html',
  styleUrl: './alerts-filtering-form.component.scss',
  imports: [
    PanelModule,
    SelectButtonModule,
    SelectModule,
    ChipModule,
    InputTextModule,
    InputGroupModule,
    InputGroupAddonModule,
    ButtonModule,
    FormsModule,
    FieldsetModule,
    ReactiveFormsModule,
  ],
})
export class AlertsFilteringFormComponent {
  protected form = new FormGroup({
    status: new FormControl<AlertStatus | null>(null),
    dataSourceId: new FormControl<string | null>(null),
    tags: new FormControl<string[]>([]),
  });

  protected tagToAdd: string = '';
  protected statuses = Object.values(AlertStatus);

  protected dataSources: DataSource[] = [];
  @Output('filter') filterEmitter = new EventEmitter<AlertsFilter>();

  protected get tags(): string[] {
    return this.form.get('tags')?.value || [];
  }

  constructor() {
    this.loadEssentials();
    this.form.valueChanges.subscribe((value) => {
      this.filterEmitter.emit({
        status: value.status || undefined,
        dataSourceId: value.dataSourceId || undefined,
        tags: value.tags || [],
      });
    });
  }

  protected loadEssentials() {
    // TODO: call API
    from([[FAKE_DATASOURCE]]).subscribe(
      (dataSources) => (this.dataSources = dataSources),
    );
  }

  protected removeTag(tag: string) {
    const tags = this.tags.filter((t) => t !== tag);
    this.form.get('tags')?.setValue(tags);
  }

  protected addTag(tag: string) {
    const tags = this.tags;
    if (tag && tags.indexOf(tag) === -1) {
      tags.push(tag);
      this.form.get('tags')?.setValue(tags);
    }
  }
}
