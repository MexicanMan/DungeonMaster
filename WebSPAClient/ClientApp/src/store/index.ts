import * as Auth from './menu/AuthReducer';
import * as Leaderboard from './menu/LeaderboardReducer';
import * as Loader from './helpers/LoaderReducer';
import * as Field from './game/FieldReducer';
import * as Controller from './game/ControllerReducer';
import * as Log from './game/LogReducer';
import { reducer as oidcReducer } from 'redux-oidc';
import { User } from 'oidc-client';

interface OidcState {
    isLoadingUser: boolean;
    user: User;
}

// The top-level state object
export interface ApplicationState {
    auth: Auth.AuthState | undefined;
    loader: Loader.LoaderState | undefined;
    field: Field.FieldState | undefined;
    controller: Controller.ControllerState | undefined;
    leaderboard: Leaderboard.LeaderboardState | undefined;
    log: Log.LogState | undefined;
    oidc: OidcState;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    auth: Auth.authReducer,
    loader: Loader.loaderReducer,
    field: Field.fieldReducer,
    controller: Controller.controllerReducer,
    leaderboard: Leaderboard.leaderboardReducer,
    log: Log.logReducer,
    oidc: oidcReducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
