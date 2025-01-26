import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TicketsComponent } from './tickets.component';
import { TicketDetailsComponent } from './ticket-details.component';

const routes: Routes = [
    {
        path: '',
        component: TicketsComponent,
        pathMatch: 'full',
    },
    {
        path: 'details',
        component: TicketDetailsComponent,
        pathMatch: 'full',
    },
    {
        path: 'view',
        component: TicketDetailsComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TicketRoutingModule {}
