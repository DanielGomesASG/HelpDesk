import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { StandardMessagesServiceProxy, CreateOrEditStandardMessageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditStandardMessageModal',
    templateUrl: './create-or-edit-standardMessage-modal.component.html',
})
export class CreateOrEditStandardMessageModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    standardMessage: CreateOrEditStandardMessageDto = new CreateOrEditStandardMessageDto();

    constructor(
        injector: Injector,
        private _standardMessagesServiceProxy: StandardMessagesServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(standardMessageId?: number): void {
        if (!standardMessageId) {
            this.standardMessage = new CreateOrEditStandardMessageDto();
            this.standardMessage.id = standardMessageId;

            this.active = true;
            this.modal.show();
        } else {
            this._standardMessagesServiceProxy.getStandardMessageForEdit(standardMessageId).subscribe((result) => {
                this.standardMessage = result;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._standardMessagesServiceProxy
            .createOrEdit(this.standardMessage)
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
