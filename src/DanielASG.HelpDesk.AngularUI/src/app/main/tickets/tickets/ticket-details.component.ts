import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { CreateOrEditTicketDto, DepartmentDto, MessageTypeDto, StatusDto, TicketMessageDto, TicketsServiceProxy, UserListDto, UserServiceProxy } from '../../../../shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { BreadcrumbItem } from '../../../shared/common/sub-header/sub-header.component';
import { ActivatedRoute, Router } from '@angular/router';
import { appModuleAnimation } from '../../../../shared/animations/routerTransition';
import { LocalStorageService } from '../../../../shared/utils/local-storage.service';
import { AppConsts } from '../../../../shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs';
import { AppStatus } from '../../../../shared/AppEnums';
import { id } from '@swimlane/ngx-charts';

@Component({
  templateUrl: './ticket-details.component.html',
    styleUrl: './ticket-details.component.css',
    animations: [appModuleAnimation()],
})
export class TicketDetailsComponent extends AppComponentBase implements OnInit {
    @ViewChild('scrollContainer') private scrollContainer!: ElementRef;
    breadcrumbs: BreadcrumbItem[] = [
        new BreadcrumbItem(this.l('Tickets'), '/app/admin/tickets/tickets'),
        new BreadcrumbItem(this.l('Details')),
    ];

    lazyLoad;
    active = false;
    sending = false;
    finishing = false;
    refreshing = false;
    isView = false;
    respond = false;

    ticket: CreateOrEditTicketDto = new CreateOrEditTicketDto();
    customerUser: UserListDto = new UserListDto();
    staffUser: UserListDto = new UserListDto();
    department: DepartmentDto = new DepartmentDto();
    status: StatusDto = new StatusDto();
    messageType: MessageTypeDto = new MessageTypeDto();
    messages: TicketMessageDto[] = [];
    downloadUrl: string = '';
    filesFileName: string;
    fileId: string;
    currentUserId: number;
    appStatus = AppStatus;

    appPriorities = [
        { name: "Baixa", code: "B" },
        { name: "Normal", code: "N" },
        { name: "Alta", code: "A" },
        { name: "Urgente", code: "U" }
    ]

    constructor(
        injector: Injector,
        private _ticketsServiceProxy: TicketsServiceProxy,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _localStorageService: LocalStorageService,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        if (Object.keys(this._activatedRoute.snapshot.queryParams).length > 1) {
            const { id, ...filteredParams } = this._activatedRoute.snapshot.queryParams;
            this.lazyLoad = { ...this.lazyLoad, ...filteredParams };
        }

        this.currentUserId = this.appSession.userId;
        this.isView = this._activatedRoute.snapshot.routeConfig.path == 'view'; 
        this.show(this._activatedRoute.snapshot.queryParams['id']);
    }

    show(ticketId?: number): void {
        this._ticketsServiceProxy
            .getTicketDetails(ticketId)
            .pipe(finalize(() => {
                setTimeout(() => {
                    this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
                }, 0);
            }))
            .subscribe((result) => {
                this.ticket = result.ticket;
                this.customerUser = result.ticket.customerUser;
                this.staffUser = result.ticket.staffUser;
                this.department = result.ticket.department;
                this.status = result.ticket.status
                this.messageType = result.ticket.messageType
                this.messages = result.messages;
                this.filesFileName = result.filesFileName;

                this.downloadUrl = this.getDownloadUrl(this.ticket.files)
                this.setUsersProfilePictureUrl([this.customerUser, this.staffUser])
                this.active = true;
            });
    }

    downloadFile() {
        window.location.href = this.downloadUrl;
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    standardMessage: string = ""
    answer: string = "";
    send(): void {
        if (!this.answer || this.answer.trim().length === 0) return;

        this.ticket.message = this.answer;
        this.sending = true

        this._ticketsServiceProxy
            .insertTicketMessage(this.ticket)
            .subscribe(x => {
                window.location.reload();
            })
    }

    onChange(event) {
        if (event) {
            this.answer = event;
        } else {
            this.answer = "";
        }
    }

    finish(): void {
        this.message.confirm('', this.l('TicketWillBeFinished') + '\n\n' + this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this.finishing = true

                this._ticketsServiceProxy
                    .finish(this.ticket)
                    .subscribe(x => {
                        this.back()
                    });
            };
        });
    }

    back() {
        this._router.navigate(['/app/main/tickets/tickets'], {
            queryParams: { ...this.lazyLoad }
        });
    }

    setUsersProfilePictureUrl(users: UserListDto[]): void {
        for (let i = 0; i < users.length; i++) {
            let user = users[i];
            this._localStorageService.getItem(AppConsts.authorization.encrptedAuthTokenName, function (err, value) {
                let profilePictureUrl =
                    AppConsts.remoteServiceBaseUrl +
                    '/Profile/GetProfilePictureByUser?userId=' +
                    user.id +
                    '&' +
                    AppConsts.authorization.encrptedAuthTokenName +
                    '=' +
                    encodeURIComponent(value.token);
                user.profilePictureUrl = profilePictureUrl;
            });
        }
    }

    getPriorityName(): string {
        if (!this.ticket || !this.ticket.priority) {
            return '-';
        }

        const priority = this.appPriorities.find(x => x.code === this.ticket.priority);
        return priority ? priority.name : '-';
    }

    refreshPage() {
        this.refreshing = true;

        this.restartCountdown();

        this.refreshing = false;
    }

    countdown: number = 30;
    countdownActive: boolean = false;
    intervalId: any;
    startCountdown() {
        this.intervalId = setInterval(() => {
            if (!this.countdownActive) {
                return;
            }

            this.countdown--;

            if (this.countdown === 0) {
                clearInterval(this.intervalId);
                this.show(this._activatedRoute.snapshot.queryParams['id']);
                this.restartCountdown();
            }
        }, 1000);
    }

    restartCountdown() {
        clearInterval(this.intervalId);
        this.countdown = 30;
        this.countdownActive = true;

        this.startCountdown();
    }

    stopCountdown() {
        clearInterval(this.intervalId);
        this.countdown = 30;
        this.countdownActive = false;
    }
}
