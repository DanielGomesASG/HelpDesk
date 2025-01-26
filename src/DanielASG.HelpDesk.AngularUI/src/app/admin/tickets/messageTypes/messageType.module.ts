import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { MessageTypeRoutingModule } from './messageType-routing.module';
import { MessageTypesComponent } from './messageTypes.component';
import { CreateOrEditMessageTypeModalComponent } from './create-or-edit-messageType-modal.component';

@NgModule({
    declarations: [MessageTypesComponent, CreateOrEditMessageTypeModalComponent],
    imports: [AppSharedModule, MessageTypeRoutingModule, AdminSharedModule],
})
export class MessageTypeModule {}
