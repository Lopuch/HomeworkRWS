import {Component, Input, OnInit} from "@angular/core";
import {Translator} from "../../../models/translator";
import {ActionSheetController} from "@ionic/angular";
import {TranslatorService} from "../../../services/translator/translator.service";

@Component({
  selector: 'app-translator-detail',
  templateUrl: './translator-detail.component.html',
  styleUrls: ['./translator-detail.component.scss'],
})
export class TranslatorDetailComponent  implements OnInit {

  @Input() translator!: Translator;

  constructor(
    private translatorService: TranslatorService,
    private actionSheetController: ActionSheetController,
  ) { }

  ngOnInit() {}

  async onSaveClick(translator: Translator) {
    await this.translatorService.save(translator);
    await this.translatorService.reload();
  }

  async onStatusClick(translator: Translator) {
    const actionSheet = await this.actionSheetController.create({
      header: "status",
      buttons: [{
        text: "Applicant",
        handler: () => {
          translator.status = "Applicant";
        }
      },{
        text: "Certified",
        handler: () => {
          translator.status = "Certified";
        }
      },{
        text: "Deleted",
        handler: () => {
          translator.status = "Deleted";
        }
      }]
    });
    await actionSheet.present();
  }
}
