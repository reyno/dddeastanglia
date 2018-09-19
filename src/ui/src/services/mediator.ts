import { HttpClient } from "aurelia-fetch-client";
import { autoinject } from "aurelia-framework";

@autoinject
export class Mediator {

  constructor(
    private httpClient: HttpClient
    ) { }

    send(name: string, request?: any): Promise<any> {



    }


}
