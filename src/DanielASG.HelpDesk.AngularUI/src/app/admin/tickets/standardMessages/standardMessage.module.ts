import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { StandardMessageRoutingModule } from './standardMessage-routing.module';
import { StandardMessagesComponent } from './standardMessages.component';
import { CreateOrEditStandardMessageModalComponent } from './create-or-edit-standardMessage-modal.component';

@NgModule({
    declarations: [StandardMessagesComponent, CreateOrEditStandardMessageModalComponent],
    imports: [AppSharedModule, StandardMessageRoutingModule, AdminSharedModule],
})
export class StandardMessageModule {}
