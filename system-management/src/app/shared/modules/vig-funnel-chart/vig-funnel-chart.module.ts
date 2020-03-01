import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VigFunnelChartComponent } from './vig-funnel-chart.component';

import 'hammerjs';

@NgModule({
  declarations: [VigFunnelChartComponent],
  imports: [
    CommonModule
  ],
  exports: [VigFunnelChartComponent]
})
export class VigFunnelChartModule { }
