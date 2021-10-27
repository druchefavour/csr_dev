import { environment }  from "./../environments/environment.prod"

export default {
    config : {
        issuer: environment.issuer,
        redirectUri: environment.issuer,
        clientId: environment.clientId,
        scopes: ['openid', 'profile', 'email'],
        addUserUrl: environment.addUserUrl,
        pkce: true
      }
    }