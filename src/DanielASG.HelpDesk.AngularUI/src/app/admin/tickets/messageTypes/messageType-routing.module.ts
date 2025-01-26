import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MessageTypesComponent } from './messageTypes.component';

const routes: Routes = [
    {
        path: '',
        component: MessageTypesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MessageTypeRoutingModule {}
