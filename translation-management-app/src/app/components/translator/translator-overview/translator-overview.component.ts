import { Component, OnInit } from '@angular/core';
import {TranslatorService} from "../../../services/translator/translator.service";

@Component({
  selector: 'app-translator-overview',
  templateUrl: './translator-overview.component.html',
  styleUrls: ['./translator-overview.component.scss'],
})
export class TranslatorOverviewComponent  implements OnInit {

  constructor(
    public translatorService: TranslatorService,
  ) { }

  async ngOnInit() {
    await this.translatorService.reload();
  }

  async onNewTranslatorClick() {
    await this.translatorService.createNew();
    await this.translatorService.reload();
  }
}
