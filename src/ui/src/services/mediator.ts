import { HttpClient, json } from "aurelia-fetch-client";
import { autoinject, Container } from "aurelia-framework";
import environment from '../environment';
import { AuthenticationContext } from "features/authentication";

@autoinject
class Mediator {

  constructor(
    private httpClient: HttpClient,
    private authenticationContext: AuthenticationContext
    ) { }

    async send(name: string, request?: any): Promise<any> {


      const token = await this.authenticationContext.acquireToken();
      const response = await this.httpClient.fetch(`${environment.api}mediator/${name}`, {
        body: (!!request && json(request)) || undefined,
        method: "POST",
        headers: {
          "content-type": "application/json",
          "authorization": "Bearer " + token
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

export var mediator = Container.instance.get(Mediator) as IMediator;
