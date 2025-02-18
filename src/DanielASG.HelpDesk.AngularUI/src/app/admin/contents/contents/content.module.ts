import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { ContentRoutingModule } from './content-routing.module';
import { ContentsComponent } from './contents.component';
import { CreateOrEditContentModalComponent } from './create-or-edit-content-modal.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { TabViewModule } from 'primeng/tabview';

@NgModule({
    declarations: [ContentsComponent, CreateOrEditContentModalComponent],
    imports: [
        AppSharedModule,
        ContentRoutingModule,
        AdminSharedModule,
        AngularEditorModule,
        TabViewModule
    ],
    exports: [ContentsComponent, CreateOrEditContentModalComponent]
})
export class ContentModule {}
