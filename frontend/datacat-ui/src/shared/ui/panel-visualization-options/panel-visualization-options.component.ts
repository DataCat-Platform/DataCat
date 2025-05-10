import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { VisualizationSettings, VisualizationType } from '../../../entities';
import { AccordionModule } from 'primeng/accordion';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { SelectModule } from 'primeng/select';

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

  protected get visualizationType() {
    return this.visualizationTypeControl.value;
  }

  protected form?: FormGroup<any>;

  constructor() {
    this.visualizationTypeControl.valueChanges.subscribe(() =>
      this.type.emit(this.visualizationType!),
    );
    this.form?.valueChanges.subscribe(() => {});
  }

  ngOnInit() {
    this.type.emit(this.visualizationType!);
  }
}
