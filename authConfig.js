// Config object to be passed to Msal on creation
const msalConfig = {
  auth: {
    clientId: "3cab413b-05db-41eb-ac76-574e8ece33f6",
    authority: "https://login.microsoftonline.com/common/",
  },
  cache: {
      cacheLocation: "sessionStorage", // This configures where your cache will be stored
      storeAuthStateInCookie: false, // Set this to "true" if you are having issues on IE11 or Edge
  },
  system: {
      loggerOptions: {
          loggerCallback: (level, message, containsPii) => {
              if (containsPii) {	
                  return;	
              }
              switch (level) {	
                  case msal.LogLevel.Error:	
                      console.error(message);	
                      return;	
                  case msal.LogLevel.Info:	
                      console.info(message);	
                      return;	
                  case msal.LogLevel.Verbose:	
                      console.debug(message);	
                      return;	
                  case msal.LogLevel.Warning:	
                      console.warn(message);	
                      return;	
              }
          }
      }
  }
};

// Add here scopes for id token to be used at MS Identity Platform endpoints.
const loginRequest = {
  scopes: ["User.Read"]
};

// Add here the endpoints for MS Graph API services you would like to use.
const graphConfig = {
  graphMeEndpoint: "https://graph.microsoft.com/v1.0/me",
};


const silentRequest = {
  scopes: ["openid", "profile", "User.Read"]
};

const logoutRequest = {}
