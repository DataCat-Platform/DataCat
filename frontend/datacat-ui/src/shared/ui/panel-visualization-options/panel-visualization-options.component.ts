import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { AccordionModule } from 'primeng/accordion';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { SelectModule } from 'primeng/select';
import { CheckboxModule } from 'primeng/checkbox';
import { createOptionsForm } from './forms';
import {
  LegendOptionsComponent,
  TooltipOptionsComponent,
} from './option-groups';
import { TitleOptionsComponent } from './option-groups/title-options/title-options.component';

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
    LegendOptionsComponent,
    TitleOptionsComponent,
    TooltipOptionsComponent,
  ],
})
export class PanelVisualizationOptionsComponent implements OnInit {
  @Output() public onOptionsChange = new EventEmitter<
    [VisualizationType, VisualizationSettings]
  >();

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
    return this.optionsForm.value;
  }

  protected optionsForm: FormGroup<any> = new FormGroup({});

  protected optionsGroup(name: string): FormGroup<any> {
    return this.optionsForm?.get(name) as FormGroup<any>;
  }

  constructor() {
    this.updateOptionsForm();

    this.visualizationTypeControl.valueChanges.subscribe(() => {
      this.updateOptionsForm();
      this.emit();
    });
    this.optionsForm?.valueChanges.subscribe(() => {
      this.emit();
    });
  }

  ngOnInit() {
    this.emit();
  }

  private emit() {
    this.onOptionsChange.emit([
      this.visualizationType!,
      this.visualizationSettings!,
    ]);
  }

  private updateOptionsForm() {
    this.optionsForm = createOptionsForm(this.visualizationType);
  }
}
