import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AppTranslationModule } from '../../app.translation.module';
import { NgaModule } from '../../theme/nga.module';

import { HomeComponent } from './home.component';
import { routing } from './home.routing';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        AppTranslationModule,
        NgaModule,
        routing
    ],
    declarations: [
        HomeComponent
    ],
    providers: [

    ]
})
export class HomeModule {}
