import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { MessageTypesServiceProxy, MessageTypeDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditMessageTypeModal',
    templateUrl: './create-or-edit-messageType-modal.component.html',
})
export class CreateOrEditMessageTypeModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    messageType: MessageTypeDto = new MessageTypeDto();

    constructor(
        injector: Injector,
        private _messageTypesServiceProxy: MessageTypesServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(messageTypeId?: number): void {
        if (!messageTypeId) {
            this.messageType = new MessageTypeDto();
            this.messageType.id = messageTypeId;

            this.active = true;
            this.modal.show();
        } else {
            this._messageTypesServiceProxy.get(messageTypeId).subscribe((result) => {
                this.messageType = result;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;

        this._messageTypesServiceProxy
            .createOrEdit(this.messageType)
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
