import { NgModule } from '@angular/core';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { CustomizableDashboardModule } from '@app/shared/common/customizable-dashboard/customizable-dashboard.module';
import { ContentModule } from '../../admin/contents/contents/content.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';

@NgModule({
    declarations: [
        HomeComponent,
    ],
    imports: [
        AppSharedModule,
        AdminSharedModule,
        HomeRoutingModule,
        CustomizableDashboardModule,
        ContentModule
    ],
})
export class HomeModule { }
