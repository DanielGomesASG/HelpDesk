import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
import { AppComponentBase } from '../../../shared/common/app-component-base';
import { ContentForHomeDto, ContentsServiceProxy } from '../../../shared/service-proxies/service-proxies';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss'],
    animations: [appModuleAnimation()]
})
export class HomeComponent extends AppComponentBase {

    constructor(injector: Injector,
        private _contentsServiceProxy: ContentsServiceProxy) {
        super(injector);
    }

    contents: ContentForHomeDto[] = [];
    ngOnInit(): void {
        this.getContents();
    }


    getContents() {
        this._contentsServiceProxy.getAllForHome().subscribe(results => {
            this.contents = results;
        });
    }
}
