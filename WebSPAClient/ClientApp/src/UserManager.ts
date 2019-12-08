/* eslint-disable @typescript-eslint/camelcase */
import { createUserManager } from 'redux-oidc';
import { UserManagerSettings } from 'oidc-client';

const userManagerConfig: UserManagerSettings = {
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

const userManager = createUserManager(userManagerConfig);

export default userManager;