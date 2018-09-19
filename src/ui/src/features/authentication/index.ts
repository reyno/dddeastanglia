import { FrameworkConfiguration } from 'aurelia-framework';
import { AuthenticationContext } from './authentication-context';
export * from './authentication-context';

export async function configure(config: FrameworkConfiguration) {

  const auth: AuthenticationContext = config.container.get(AuthenticationContext);

  await auth.initialise();

}
