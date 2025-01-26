import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { TicketsServiceProxy, CreateOrEditTicketDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileUploader, FileUploaderOptions } from '@node_modules/ng2-file-upload';
import { AbpSessionService, IAjaxResponse, TokenService } from '@node_modules/abp-ng2-module';
import { AppConsts } from '@shared/AppConsts';

import { HttpClient } from '@angular/common/http';
import { Session } from 'inspector';

@Component({
    selector: 'createOrEditTicketModal',
    templateUrl: './create-or-edit-ticket-modal.component.html',
})
export class CreateOrEditTicketModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
    bind = false;

    ticket: CreateOrEditTicketDto = new CreateOrEditTicketDto();

    filesFileUploader: FileUploader;
    filesFileToken: string;
    filesFileName: string;
    filesFileAcceptedTypes: string = '';
    downloadUrl: string = '';
    @ViewChild('Ticket_filesLabel') ticket_filesLabel: ElementRef;

    constructor(
        injector: Injector,
        private _ticketsServiceProxy: TicketsServiceProxy,
        private _dateTimeService: DateTimeService,
        private _tokenService: TokenService,
        private _http: HttpClient,
    ) {
        super(injector);
    }

    show(ticketId?: number, bind?: boolean): void {
        this.bind = bind || false;

        if (!ticketId) {
            this.ticket = new CreateOrEditTicketDto();
            this.ticket.id = ticketId;
            this.ticket.customerUserId = this.appSession.userId;

            this.filesFileName = null;

            this.active = true;
            this.modal.show();
        } else {
            this._ticketsServiceProxy.getTicketForEdit(ticketId).subscribe((result) => {
                this.ticket = result.ticket;
                this.ticket.customerUserId ??= this.appSession.userId;

                this.filesFileName = result.filesFileName;

                this.active = true;
                this.modal.show();

                if (this.ticket.files) {
                    this.downloadUrl = this.getDownloadUrl(this.ticket.files);
                    this.downloadFile();
                }
            });
        }

        this.filesFileUploader = this.initializeUploader(
            AppConsts.remoteServiceBaseUrl + '/Tickets/UploadfilesFile',
            (fileToken) => (this.filesFileToken = fileToken),
        );
    }

    downloadFile() {
        if (this.downloadUrl) {
            this._http.get(this.downloadUrl, { responseType: 'blob' }).subscribe({
                next: (response) => {
                    const file = new File([response], this.filesFileName, { type: response.type });

                    this.onSelectFilesFile(file);
                },
                error: (error) => {
                    console.error('Erro ao baixar o arquivo', error);
                },
                complete: () => {
                    console.log('Download completo');
                }
            });
        }
    }

    save(): void {
        if (!this.ticket.message || this.ticket.message.trim().length === 0) return;
        this.saving = true;

        this.ticket.filesToken = this.filesFileToken;

        this._ticketsServiceProxy
            .createOrEdit(this.ticket)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit({});
            });
    }

    bindStaff(): void {
        this.saving = true;

        this.ticket.filesToken = this.filesFileToken;

        this._ticketsServiceProxy
            .bind(this.ticket)
            .pipe(
                finalize(() => {
                    this.saving = false;
                }),
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit({});
            });
    }

    onSelectFilesFile(fileInput: any): void {
        let selectedFile = fileInput.target ? <File>fileInput.target.files[0] : fileInput;

        this.filesFileUploader.clearQueue();
        this.filesFileUploader.addToQueue([selectedFile]);
        this.filesFileUploader.uploadAll();
    }

    removeFilesFile(): void {
        this.message.confirm(this.l('DoYouWantToRemoveTheFile'), this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._ticketsServiceProxy.removeFilesFile(this.ticket.id).subscribe(() => {
                    abp.notify.success(this.l('SuccessfullyDeleted'));
                    this.filesFileToken = null;
                    this.filesFileName = null;
                });
            }
        });
    }

    initializeUploader(url: string, onSuccess: (fileToken: string) => void): FileUploader {
        let uploader = new FileUploader({ url: url });

        let _uploaderOptions: FileUploaderOptions = {
            url: url,
            autoUpload: false,
            authToken: 'Bearer ' + this._tokenService.getToken(),
            removeAfterUpload: true,
        };

        uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        uploader.onSuccessItem = (item, response, status) => {
            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success && resp.result.fileToken) {
                onSuccess(resp.result.fileToken);
            } else {
                this.message.error(resp.result.message);
            }
        };

        uploader.setOptions(_uploaderOptions);
        return uploader;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {
        this._http.get(AppConsts.remoteServiceBaseUrl + '/tickets/GetFilesFileAllowedTypes').subscribe((data: any) => {
            if (!data || !data.result) {
                return;
            }

            let list = data.result as string[];
            if (list.length == 0) {
                return;
            }

            for (let i = 0; i < list.length; i++) {
                this.filesFileAcceptedTypes += '.' + list[i] + ',';
            }
        });
    }

    onChange(event) {
        if (!event) {
            this.ticket.message = undefined;
            return;
        }

        if (event && !event.code) return;

        this.ticket.message = event.code;
    }
}
