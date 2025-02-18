import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { DepartmentsServiceProxy, CreateOrEditDepartmentDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditDepartmentModal',
    templateUrl: './create-or-edit-department-modal.component.html',
})
export class CreateOrEditDepartmentModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    department: CreateOrEditDepartmentDto = new CreateOrEditDepartmentDto();

    constructor(
        injector: Injector,
        private _departmentsServiceProxy: DepartmentsServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(departmentId?: number): void {
        if (!departmentId) {
            this.department = new CreateOrEditDepartmentDto();
            this.department.id = departmentId;

            this.active = true;
            this.modal.show();
        } else {
            this._departmentsServiceProxy.getDepartmentForEdit(departmentId).subscribe((result) => {
                this.department = result;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._departmentsServiceProxy
            .createOrEdit(this.department)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
