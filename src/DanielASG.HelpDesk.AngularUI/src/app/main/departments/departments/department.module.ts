import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { DepartmentRoutingModule } from './department-routing.module';
import { DepartmentsComponent } from './departments.component';
import { CreateOrEditDepartmentModalComponent } from './create-or-edit-department-modal.component';

@NgModule({
    declarations: [DepartmentsComponent, CreateOrEditDepartmentModalComponent],
    imports: [AppSharedModule, DepartmentRoutingModule, AdminSharedModule],
})
export class DepartmentModule {}
