import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentComponent } from './student.component';


const routes: Routes = [{
  path: "",
  component: StudentComponent,
  children: [
    {
      path: "main",
      loadChildren: () =>
        import("./main/student-main.module").then(
          m => m.StudentMainModule
        )
    },{ path: "", redirectTo: "main", pathMatch: "full" }
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
