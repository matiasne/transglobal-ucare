import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ServerErrorValidationComponent } from './server-error-validation.component';

const routes: Routes = [
    {
    path: '', component: ServerErrorValidationComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ServerErrorValidationRoutingModule {
}
