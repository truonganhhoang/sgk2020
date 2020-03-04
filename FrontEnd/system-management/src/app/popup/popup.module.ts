import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PopupAddTaskComponent } from './popup-add-task/popup-add-task.component';
import { DxPopupModule, DxTextBoxModule, DxValidatorModule, DxSelectBoxModule, DxNumberBoxModule, DxDateBoxModule } from 'devextreme-angular';



@NgModule({
  declarations: [PopupAddTaskComponent],
  imports: [
    CommonModule,
    DxPopupModule,
    DxTextBoxModule,
    DxValidatorModule,
    DxSelectBoxModule,
    DxNumberBoxModule,
    DxDateBoxModule
  ],
  exports:[PopupAddTaskComponent]
})
export class PopupModule { }
