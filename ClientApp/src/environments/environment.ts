// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.


export const environment = {
  production: false,
  title: 'Local Environment Heading',
  apiURL: "https://webappsdev.iltest.illinois.gov/DES/CSRTool",
  domain: 'https://ilogin-dev.illinois.gov',
  issuer: 'https://ilogin-dev.illinois.gov/oauth2/default',
  redirectUri: "https://webappsdev.iltest.illinois.gov/DES/CSRTool",
  clientId: '0oayximdaqVLZahGQ1d6',
  addUserUrl: "https://doitappoktaprod-dev.azurewebsites.us/"
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
