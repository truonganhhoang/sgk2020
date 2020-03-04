import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { DxListModule, DxContextMenuModule } from 'devextreme-angular';


@NgModule({
  declarations: [NavBarComponent],
  imports: [
    CommonModule,
    DxContextMenuModule,
    DxListModule
  ],
  exports:[
    NavBarComponent
  ]
})
export class ToolbarModule { }
