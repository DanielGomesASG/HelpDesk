import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StandardMessagesComponent } from './standardMessages.component';

const routes: Routes = [
    {
        path: '',
        component: StandardMessagesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class StandardMessageRoutingModule {}
