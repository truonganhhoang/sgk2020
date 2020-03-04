import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManagementComponent } from './management.component';


const routes: Routes = [{
  path: "",
  component: ManagementComponent,
  children: [
    {
      path: "home",
      loadChildren: () =>
        import("./../home/home.module").then(
          m => m.HomeModule
        )
    },{
      path: "student",
      loadChildren: () =>
        import("./../student/student.module").then(
          m => m.StudentModule
        )
    },{ path: "", redirectTo: "student", pathMatch: "full" }]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManagementRoutingModule { }
