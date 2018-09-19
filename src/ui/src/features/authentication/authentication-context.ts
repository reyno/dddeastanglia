import { autoinject } from 'aurelia-framework';
import * as Msal from 'msal';
import { HttpClient } from 'aurelia-fetch-client';
import environment from '../../environment';

const msalconfig = {
  clientId: "2c6a01e7-fe64-4a41-9a8c-2c0a53a54513",
  authority: "https://login.microsoftonline.com/common",
  redirectUri: location.origin
};


@autoinject
export class AuthenticationContext {

  user: any;
  userAgentApplication: Msal.UserAgentApplication;
  mediator: IMediator;

  constructor(
    private httpClient: HttpClient
  ) { }

  initialise(): Promise<any> {

    return new Promise(async (resolve, reject) => {

      this.userAgentApplication = new Msal.UserAgentApplication(
        msalconfig.clientId,
        msalconfig.authority,
        (errorDesc, token, error, tokenType) => {
        }, {
          redirectUri: msalconfig.redirectUri,
          logger: new Msal.Logger((level: Msal.LogLevel, message: string, containsPii: boolean) => {
            console.log(message);
          })
        }
      );

      if (!this.userAgentApplication.isCallback(window.location.hash)) {

        const user = <any>this.userAgentApplication.getUser();
        const token = !user
          ? await this.userAgentApplication.loginPopup([msalconfig.clientId])
          : await this.userAgentApplication.acquireTokenSilent([msalconfig.clientId])
          ;

        resolve();

      }


    });



  }


  async acquireToken(): Promise<string> {

    return new Promise<string>((resolve, reject) => {

      this.userAgentApplication
        .acquireTokenSilent([msalconfig.clientId])
        .then(
          token => resolve(token),
          error => !!error && this.userAgentApplication.acquireTokenRedirect([msalconfig.clientId])
        );

    });

  }



}

export class User extends Msal.User {
  roles: string[];


}
