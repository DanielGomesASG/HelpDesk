import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { TicketRoutingModule } from './ticket-routing.module';
import { TicketsComponent } from './tickets.component';
import { CreateOrEditTicketModalComponent } from './create-or-edit-ticket-modal.component';
import { TabViewModule } from 'primeng/tabview';
import { TicketDetailsComponent } from './ticket-details.component';
import { TabMenuModule } from 'primeng/tabmenu';

@NgModule({
    declarations: [
        TicketsComponent,
        CreateOrEditTicketModalComponent,
        TicketDetailsComponent
    ],
    imports: [
        AppSharedModule,
        TicketRoutingModule,
        AdminSharedModule,
        TabViewModule,
        TabMenuModule
    ],
})
export class TicketModule {}
