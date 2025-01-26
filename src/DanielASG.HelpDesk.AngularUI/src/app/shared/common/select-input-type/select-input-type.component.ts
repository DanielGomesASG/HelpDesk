import { Component, EventEmitter, Injector, Input, OnChanges, OnInit, Output, SimpleChange } from '@angular/core';
import { NgSelectConfig } from '@ng-select/ng-select';
import { concat, EMPTY, Observable, of, Subject } from 'rxjs';
import { catchError, distinctUntilChanged, map, switchMap, tap, find, debounceTime, filter, take } from 'rxjs/operators';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { BaseSelectInputDto } from '../../../../shared/service-proxies/service-proxies';
import { dinamicallyService } from './dinamically-service.service';


@Component({
  selector: 'select-input',
    templateUrl: './select-input-type.component.html',
    styleUrls: ['./select-input-type.component.scss'],
})
export class SelectInputTypeComponent extends AppComponentBase implements OnInit, OnChanges {
    @Input() name: string;
    @Input() serviceName: string;
    @Input() bindValue: any;
    @Input() readonly: boolean = false;
    @Input() placeholder: string = 'Selecione uma opção';
    @Input() value: any = '';
    @Input() appendTo = '';
    @Input() required: boolean = false;
    @Input() submitted: boolean = false;
    @Output() selectedValueChange = new EventEmitter();
    @Output() valueChange = new EventEmitter();
    @Output() change = new EventEmitter();


    //inputs utilizados para filtrar registros pela empresa
    @Input() companyId: number;
    //só ira trazer caso o campo companyId estiver preenchido
    @Input() forceCompany: boolean = false;
    //zera a variavel value caso verdadeiro
    @Input() reload: boolean = false;
    //ativa opção de seleção multipla
    @Input() multiple: boolean = false;
    //altera o padrão da função que retorna os valores da lista | default:getAllForCombobox
    @Input() customFunction: string = '';
    @Input() customFunctionParam: any;

    selectedValue: any;
    list$: Observable<BaseSelectInputDto[]>;

    loading = false;
    input$ = new Subject<string>();
    service: any;

    closeOnSelect: boolean = true;

    initLoading = true;
    constructor(
        injector: Injector,
        private config: NgSelectConfig
    ) {
        super(injector);
        this.config.notFoundText = 'Nenhum registro encontrado...';
        this.config.typeToSearchText = 'Busque por alguma palavra chave'
    }

    async ngOnInit(): Promise<void> {

        if (dinamicallyService.hasOwnProperty(this.serviceName)) {
            this.service = this.injector.get<any>(dinamicallyService[this.serviceName]);
        } else {
            throw new Error(`Não foi possivel localizar o serviço '${this.serviceName}'`);
        }

        await this.get();
        if (!this.value)
            this.value = '';

        //if (this.multiple) {
        //    this.closeOnSelect = false;
        //}

        this.initLoading = false;
    }

    //se vier atualização de fora, ele atualiza o valor selecionado
    ngOnChanges(changes: { [key: string]: SimpleChange }) {
        if (changes.hasOwnProperty('value')) {
            if (changes['value'].currentValue === undefined) {
                delete this.selectedValue;
                delete this.value;
                this.get();
            }

            if (changes['value'].currentValue && !this.selectedValue ||
               (this.selectedValue && this.selectedValue[this.bindValue] != changes['value'].currentValue)) {
                this.get();
            }
        }

        if (changes.hasOwnProperty('companyId')) {
            if (this.reload && !this.initLoading) {
                delete this.selectedValue;
                this.changeValue();
            }

            this.get();
        }
    }

    //função para buscar valor e atualizar na tela
    changeSelectedValue(items) {
        if (this.bindValue) {
            if (!this.selectedValue || this.selectedValue[this.bindValue] != this.value) {
                if (this.multiple) {
                    this.selectedValue = items.filter(item => this.value.indexOf(item[this.bindValue]) > -1);
                } else {
                    this.selectedValue = items.find(item => item[this.bindValue] == this.value);
                }
            }
        }
    }

    //função para setar as alterações ao selecionar um valor
    changeValue() {
        if (this.selectedValue) {
            if (this.bindValue) {
                if (this.multiple) {
                    this.value = [];
                    this.selectedValue.forEach(item => this.value.push(item[this.bindValue]));
                } else {
                    this.value = this.selectedValue[this.bindValue];
                }
            } else {
                this.value = this.selectedValue;
            }
        } else {
            this.value = undefined;
        }

        this.selectedValueChange.emit(this.selectedValue);
        this.valueChange.emit(this.value);
        this.change.emit(this.value);
    }


    //buscar a lista com valores iniciais
    async get(): Promise<void> {
        if (this.forceCompany && !this.companyId || !this.service)
            return;

        var initialIds = !this.value || this.multiple ? undefined : Array.isArray(this.value) ? this.value : [this.value];
        var result = await this.getAllForSelectInput('', initialIds).toPromise();
        if ((initialIds && result.length) || (this.value && this.multiple)) {
            this.changeSelectedValue(result);
        }

        this.list$ = concat(
            of(result), // default items
            this.input$.pipe(
                filter(res => {
                    return res !== null
                }),
                distinctUntilChanged(),
                debounceTime(800),
                tap(() => this.loading = true),
                switchMap(term =>  this.getAllForSelectInput(term, []).pipe(
                    catchError(() => of(result)), // empty list on error
                        tap(() => this.loading = false)
                    )
                )
            )
        );
    }

    getAllForSelectInput(term, initialId = undefined): Observable<BaseSelectInputDto[]> {
        if (this.companyId) {
            return this.service["getAllForSelectInputByCompanyId"](term, this.companyId);
        } else
            if (!this.customFunction) {
                return this.service["getAllForSelectInput"](term, initialId);
            } else {
                if (!this.customFunctionParam) {
                    return this.service[this.customFunction](term, initialId);
                } else {
                    return this.service[this.customFunction](term, undefined, this.customFunctionParam);
                }
            }
    }


}

