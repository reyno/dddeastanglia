import { HttpClient, json } from "aurelia-fetch-client";
import { autoinject } from "aurelia-framework";
import environment from '../environment';

@autoinject
export class Mediator {

  constructor(
    private httpClient: HttpClient
    ) { }

    async send(name: string, request?: any): Promise<any> {

      const response = await this.httpClient.fetch(`${environment.api}_mediator/${name}`, {
        body: (!!request && json(request)) || undefined,
        method: "POST",
        headers: {
          "content-type": "application/json"
        }
      })
  
  
      if (!response.ok) {
        const error = await response.json();
        if (!!error.validationErrors) {
          return Promise.reject(new ValidationError(error));
        } else {
          return Promise.reject(new Error());
        }
      }
  
      return await response.json()

    }


}


class ValidationError extends Error {
  validationErrors: any[];
  constructor(error: { validationErrors: any[] }) {
    super();
    this.validationErrors = error.validationErrors;
  }
}

