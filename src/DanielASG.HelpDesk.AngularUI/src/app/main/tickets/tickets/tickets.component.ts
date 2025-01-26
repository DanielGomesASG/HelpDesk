import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild, AfterViewInit, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TicketsServiceProxy, ITicketDto, TicketDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditTicketModalComponent } from './create-or-edit-ticket-modal.component';

import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent, MenuItem } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppStatus } from '../../../../shared/AppEnums';
import { Location } from '@angular/common';
import { stringify } from 'querystring';
import { finalize } from 'rxjs';
import { T } from '@fullcalendar/core/internal-common';

@Component({
    templateUrl: './tickets.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class TicketsComponent extends AppComponentBase implements OnInit, AfterViewInit {
    @ViewChild('createOrEditTicketModal', { static: true }) createOrEditTicketModal: CreateOrEditTicketModalComponent;

    @ViewChild('dataTable', { static: false }) dataTable: Table;
    @ViewChild('paginator', { static: false }) paginator: Paginator;

    event;
    lazyLoad;
    advancedFiltersAreShown = false;
    filterText = '';

    isInitialLoad: boolean = true;
    ticketDto: TicketDto = new TicketDto();
    activeItem: MenuItem;
    userId: number;
    appStatus = AppStatus;

    items: MenuItem[] | undefined = [];

    appPriorities = [
        { name: "Baixa", code: "B" },
        { name: "Normal", code: "N" },
        { name: "Alta", code: "A" },
        { name: "Urgente", code: "U" }
    ]

    constructor(
        injector: Injector,
        private _ticketsServiceProxy: TicketsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _router: Router,
        private _location: Location
    ) {
        super(injector);
    }

    ngOnInit() {
        if (this.permission.isGranted('Pages.Tickets.All')) {
            this.items.push({
                tabindex: 'all',
                label: `${this.l('Every')}`,
                icon: 'las la-mail-bulk fs-1 me-3',
                command: () => { this.userId = undefined }
            });
        }

        if (this.permission.isGranted('Pages.Tickets')) {
            this.items.push({
                tabindex: 'my',
                label: `${this.l('MyTickets')}`,
                icon: 'las la-envelope fs-1 me-3',
                command: () => { this.userId = this.appSession.user.id; }
            });
        };

        if (Object.keys(this._activatedRoute.snapshot.queryParams).length > 0) {
            this.lazyLoad = { ...this._activatedRoute.snapshot.queryParams };
            this.event = this.processParams();
        }

        this.activeItem = this.items.find(x => x.tabindex === this.lazyLoad.tab) ?? this.items[0];
        this.userId = this.activeItem.tabindex === "all" ? undefined : this.appSession.userId;
    }

    ngAfterViewInit() {
        this.getTickets(this.event);
    }

    getTickets(event?: LazyLoadEvent) {
        if (this.isInitialLoad) { this.isInitialLoad = false; return; }
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        event = this.setLocalStorage(event);
        this.primengTableHelper.showLoadingIndicator();
        this._ticketsServiceProxy
            .getAll(
                this.filterText,
                this.userId,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event),
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    setLocalStorage(event?: LazyLoadEvent) {
        var params = { ...this.dataTable.createLazyLoadMetadata(), ...event, ...{ filterText: this.filterText, tab: this.activeItem?.tabindex } };
        delete params.filters;

        if (this.lazyLoad) {
            params = Object.keys(this.lazyLoad).reduce((acc, key) => {
                acc[key] = params[key] ?? this.lazyLoad[key];
                return acc;
            }, { ...params })
        }

        params.first = params.page * parseInt(params.rows) || 0;
        localStorage.setItem(this.lazyLoad, JSON.stringify(params));
        var storedFilters = localStorage.getItem(this.lazyLoad);

        this.lazyLoad = JSON.parse(storedFilters) || {};
        this.paginator.rows = this.lazyLoad.rows;
        this.paginator.first = this.lazyLoad.first;
        this.dataTable.sortField = this.lazyLoad.sortField;
        this.dataTable.sortOrder = this.lazyLoad.sortOrder;
        this.primengTableHelper.defaultRecordsCountPerPage = this.lazyLoad.rows;

        const url = this._router.createUrlTree([], {
            relativeTo: this._activatedRoute,
            queryParams: this.lazyLoad,
        }).toString();
        this._location.go(url);

        return params;
    }

    processParams(): any {
        this.lazyLoad.rows = parseInt(this.lazyLoad.rows);
        this.lazyLoad.page = parseInt(this.lazyLoad.page);
        this.lazyLoad.first = parseInt(this.lazyLoad.first);
        this.lazyLoad.sortOrder = parseInt(this.lazyLoad.sortOrder);
        this.lazyLoad.pageCount = parseInt(this.lazyLoad.pageCount);

        var ticketProperties = Object.getOwnPropertyNames(this.ticketDto.toJSON());
        if (!ticketProperties.includes(this.lazyLoad.sortField || (this.lazyLoad.sortOrder !== (1 || -1)))) {
            this.lazyLoad.sortField = null;
            this.lazyLoad.sortOrder = null;
        };

        if (this.lazyLoad.rows) {
            var checkCount = this.primengTableHelper.predefinedRecordsCountPerPage.find(x => x === this.lazyLoad.rows);
            if (!checkCount) {
                var spliceIndex = this.primengTableHelper.predefinedRecordsCountPerPage.findIndex(x => x > this.lazyLoad.rows);
                this.primengTableHelper.predefinedRecordsCountPerPage.splice(spliceIndex, 0, this.lazyLoad.rows);
            }
        };

        if (this.lazyLoad.filterText) {
            this.filterText = this.lazyLoad.filterText
        };

        if ((this.lazyLoad.first / this.lazyLoad.rows) !== this.lazyLoad.page) {
            this.lazyLoad.page = 0;
            this.lazyLoad.first = 0;
        };

        var event = {
            first: this.lazyLoad.first || 0,
            page: this.lazyLoad.page || 0,
            pageCount: this.lazyLoad.pageCount || 0,
            rows: this.lazyLoad.rows || 10,
            sortOrder: this.lazyLoad.sortOrder,
            sortField: this.lazyLoad.sortField
        };

        return event;
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createTicket(): void {
        this.createOrEditTicketModal.show();
    }

    deleteTicket(ticket: TicketDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._ticketsServiceProxy.delete(ticket.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    getDownloadUrl(id: string): string {
        return AppConsts.remoteServiceBaseUrl + '/File/DownloadBinaryFile?id=' + id;
    }

    resetFilters(): void {
        this.filterText = '';

        this.getTickets();
    }
    onActiveItemChange(event?: any) {
        this.activeItem = event;

        this.getTickets()
    }

    getPriorityName(priorityCode) {
        if (!priorityCode) {
            return '-';
        }

        const priority = this.appPriorities.find(x => x.code === priorityCode);
        return priority ? priority.name : '-';
    }

    ticketDetails(ticketId: number) {
        this._router.navigate(['/app/main/tickets/tickets/details'], {
            queryParams: { ...this.lazyLoad, id: ticketId }
        });
    }

    ticketView(ticketId: number) {
        this._router.navigate(['/app/main/tickets/tickets/view'], {
            queryParams: { ...this.lazyLoad, id: ticketId }
        });
    }
}
