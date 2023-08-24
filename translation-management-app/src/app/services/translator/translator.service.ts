import {Injectable} from "@angular/core";
import {environment} from "../../../environments/environment";
import {firstValueFrom, Subject} from "rxjs";
import {Translator} from "../../models/translator";
import {HttpClient} from "@angular/common/http";

@Injectable({
  providedIn: "root"
})
export class TranslatorService {

  apiUrl = environment.apiUrl + "translators";

  private translators = new Subject<Translator[]>;
  public translators$ = this.translators.asObservable();

  constructor(
    private http: HttpClient,
  ) {
  }

  async reload() {
    const res = await firstValueFrom(
      this.http.get<{ items: Translator[] }>(this.apiUrl)
    )

    this.translators.next(res.items);
  }

  async save(translator: Translator){
    await firstValueFrom(
      this.http.put<Translator>(`${this.apiUrl}/${translator.id}`,
        translator
        )
    )
  }

  async createNew(){
    await firstValueFrom(
      this.http.post(this.apiUrl,
        {
          name: "New translator",
          hourlyRate: 1,
          status: "Applicant",
          creditCardNumber: "1234 5678 9101"
        }
        )
    );
  }
}
