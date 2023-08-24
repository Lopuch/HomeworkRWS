import {NgModule} from "@angular/core";
import {TranslatorOverviewComponent} from "./translator-overview/translator-overview.component";
import {CommonModule} from "@angular/common";
import {IonicModule} from "@ionic/angular";
import {FormsModule} from "@angular/forms";
import {TranslatorDetailComponent} from "./translator-detail/translator-detail.component";

@NgModule({
  declarations: [
    TranslatorOverviewComponent,
    TranslatorDetailComponent,
  ],
  imports: [
    CommonModule,
    IonicModule,
    FormsModule,
  ],
  exports: [
    TranslatorOverviewComponent
  ]
})
export class TranslatorModule {
}
