import { Component, ViewChild, Injector, Output, EventEmitter, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { ContentsServiceProxy, ContentDto, RoleServiceProxy, UserRoleDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';
import { map as _map, filter as _filter } from 'lodash-es';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';

@Component({
    selector: 'createOrEditContentModal',
    templateUrl: './create-or-edit-content-modal.component.html',
})
export class CreateOrEditContentModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    content: ContentDto = new ContentDto();

    constructor(
        injector: Injector,
        private _contentsServiceProxy: ContentsServiceProxy,
        private _rolesServiceProxy: RoleServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    html: string = '';
    show(contentId?: number): void {
        if (!contentId) {
            this.content = new ContentDto();
            this.content.id = contentId;
            this.html = '';

            this.getAllRoles();

            this.active = true;
            this.modal.show();
        } else {
            this._contentsServiceProxy.get(contentId).subscribe((result) => {
                this.content = result;
                this.html = result.contentHtml;

                this.getAllRoles();

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
        this.saving = true;
        this.content.contentHtml = this.html;
        this.content.roleIds = _map(
            _filter(this.roles, { isAssigned: true }),
            (role) => role.roleId
        );

        this._contentsServiceProxy
            .createOrEdit(this.content)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.modalSave.emit(null);
                this.close();
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    employerIds = [];
    roles: UserRoleDto[] = [];
    getAllRoles() {
        this._rolesServiceProxy
            .getAllRolesForSelect()
            .subscribe(roles => {
                this.roles = roles;
                this.roles.forEach(x => {
                    x.isAssigned = this.content.roleIds.includes(x.roleId)
                });
            });
    }

    ngOnInit(): void { }

    editorConfig: AngularEditorConfig = {
        editable: true,
        spellcheck: true,
        height: 'auto',
        minHeight: '400px',
        maxHeight: 'auto',
        width: 'auto',
        minWidth: '0',
        translate: 'yes',
        enableToolbar: true,
        showToolbar: true,
        placeholder: this.l('EnterTextOrHtml'),
        defaultParagraphSeparator: '',
        defaultFontName: '',
        defaultFontSize: '',
        fonts: [
            { class: 'arial', name: 'Arial' },
            { class: 'times-new-roman', name: 'Times New Roman' },
            { class: 'calibri', name: 'Calibri' },
            { class: 'comic-sans-ms', name: 'Comic Sans MS' }
        ],
        customClasses: [
            {
                name: 'quote',
                class: 'quote',
            },
            {
                name: 'redText',
                class: 'redText'
            },
            {
                name: 'titleText',
                class: 'titleText',
                tag: 'h1',
            },
        ],
        uploadWithCredentials: false,
        sanitize: false,
        toolbarPosition: 'top',
        toolbarHiddenButtons: [
            ['bold', 'italic'],
            ['fontSize']
        ]
    };
}
