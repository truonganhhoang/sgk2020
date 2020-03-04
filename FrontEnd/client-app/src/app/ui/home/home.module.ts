import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { PopupModule } from 'src/app/popup/popup.module';
import { DirectiveModule } from 'src/app/directive/directive.module';
import { VigFunnelChartModule } from 'src/app/shared/modules/vig-funnel-chart/vig-funnel-chart.module';


@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    HomeRoutingModule,
    PopupModule,
    DirectiveModule,
    VigFunnelChartModule
  ]
})
export class HomeModule { }
