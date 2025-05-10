import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { AccordionModule } from 'primeng/accordion';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { createOptionsForm } from './forms';

@Component({
  standalone: true,
  selector: 'datacat-panel-visualization-options',
  templateUrl: './panel-visualization-options.component.html',
  styleUrl: './panel-visualization-options.component.scss',
  imports: [
    AccordionModule,
    ReactiveFormsModule,
    InputTextModule,
    PanelModule,
    SelectModule,
    CheckboxModule,
    SelectModule,
  ],
})
export class PanelVisualizationOptionsComponent implements OnInit {
  @Output() public type = new EventEmitter<VisualizationType>();
  @Output() public settings = new EventEmitter<VisualizationSettings>();

  protected VisualizationType = VisualizationType;
  protected selectableVisualizationTypes = Object.values(
    VisualizationType,
  ).filter((t) => t !== VisualizationType.UNKNOWN);

  protected visualizationTypeControl = new FormControl<VisualizationType>(
    VisualizationType.LINE,
  );

  protected get visualizationType(): VisualizationType | null {
    return this.visualizationTypeControl.value;
  }

  protected get visualizationSettings(): VisualizationSettings {
    return {};
  }

  protected optionsForm?: FormGroup<any>;

  constructor() {
    this.visualizationTypeControl.valueChanges.subscribe(() =>
      this.type.emit(this.visualizationType!),
    );
    this.optionsForm?.valueChanges.subscribe(() => {});
  }

  ngOnInit() {
    this.type.emit(this.visualizationType!);
    this.settings.emit(this.visualizationSettings);
  }

  private updateOptionsForm() {
    this.optionsForm = createOptionsForm(this.visualizationType);
  }
}
