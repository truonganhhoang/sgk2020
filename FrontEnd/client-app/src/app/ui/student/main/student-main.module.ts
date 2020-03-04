import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StudentMainRoutingModule } from './student-main-routing.module';
import { StudentMainComponent } from './student-main.component';


@NgModule({
  declarations: [StudentMainComponent],
  imports: [
    CommonModule,
    StudentMainRoutingModule
  ]
})
export class StudentMainModule { }
