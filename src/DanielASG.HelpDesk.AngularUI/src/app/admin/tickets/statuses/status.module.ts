import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
import {StatusRoutingModule} from './status-routing.module';
import {StatusesComponent} from './statuses.component';
import {CreateOrEditStatusModalComponent} from './create-or-edit-status-modal.component';




@NgModule({
    declarations: [
        StatusesComponent,
        CreateOrEditStatusModalComponent,
    ],
    imports: [AppSharedModule, StatusRoutingModule , AdminSharedModule ],
    
})
export class StatusModule {
}
