import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SelectInputTypeComponent } from './select-input-type.component';
import { NgOptionHighlightModule } from '@ng-select/ng-option-highlight';

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        NgSelectModule,
        NgOptionHighlightModule
    ],
    declarations: [
        SelectInputTypeComponent,
    ],
    exports: [
        SelectInputTypeComponent
    ]
})
export class SelectInputTypeModule { }
