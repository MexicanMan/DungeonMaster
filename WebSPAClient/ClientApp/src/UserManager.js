"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
/* eslint-disable @typescript-eslint/camelcase */
var redux_oidc_1 = require("redux-oidc");
var userManagerConfig = {
    client_id: 'spa',
    redirect_uri: 'https://localhost:8081/oacallback',
    response_type: 'code',
    client_secret: 'secret',
    scope: "openid profile api1",
    authority: 'http://localhost:5010/',
    automaticSilentRenew: true,
    filterProtocolClaims: true,
    loadUserInfo: true,
    monitorSession: true,
};
var userManager = redux_oidc_1.createUserManager(userManagerConfig);
exports.default = userManager;
//# sourceMappingURL=UserManager.js.map