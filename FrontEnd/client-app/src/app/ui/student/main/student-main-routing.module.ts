import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentMainComponent } from './student-main.component';


const routes: Routes = [{
  path: "",
  component: StudentMainComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentMainRoutingModule { }
