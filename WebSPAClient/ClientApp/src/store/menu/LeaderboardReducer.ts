import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR } from '../../appconfig';
import { } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LeaderboardState {
    error: string;
    isNewRegistered: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

/*interface AuthSuccessAction {
    type: typeof AUTH_SUCCESS;
}*/

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
//type KnownAction = AuthSuccessAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export interface LeaderboardResponse {
    players: PlayerLeaderboard[];
}

export interface PlayerLeaderboard {
    username: string;
    treasureCount: number;
    isDead: boolean;
}

function setSessionItems(auth_token: string, id: string, username: string) {
    sessionStorage.setItem('auth_token', auth_token);
    sessionStorage.setItem('id', id);
    sessionStorage.setItem('username', username);
}

export const actionCreators = {
    requestAuth: (username: string, pwd: string): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        fetch(`${GATEWAY_ADDR}/api/auth/`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: pwd
            })
        })
            .then(response => {
                if (response.ok)
                    return response.json() as Promise<LeaderboardResponse>;
                else
                    throw (response.json() as Promise<ErrorResponse>);
            })
            .then(data => {
                //setSessionItems(data.auth_token, data.id, username);

                dispatch(loaderActionCreators.response());
                dispatch(actionCreators.moveToMainMenu());
            })
            .catch((error: Promise<ErrorResponse>) => {
                error.then(error => {
                    dispatch(loaderActionCreators.response())
                });
            })
            .catch(error => {
                console.log(error);
            });
    },

    requestReg: (username: string, pwd: string): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        fetch(`${GATEWAY_ADDR}/api/auth/reg/`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: pwd
            })
        })
            .then(response => {
                if (response.ok)
                    return response.json() as Promise<LeaderboardResponse>;
                else
                    throw (response.json() as Promise<ErrorResponse>);
            })
            .then(data => {
                //setSessionItems(data.auth_token, data.id, username);

                dispatch(loaderActionCreators.response());
            })
            .catch((error: Promise<ErrorResponse>) => {
                error.then(error => {
                    dispatch(loaderActionCreators.response())
                });
            })
            .catch(error => {
                console.log(error);
            });
    },

    moveToMainMenu: () => (dispatch: Dispatch) => { dispatch(push(Path.MAIN_MENU)); },

    /*authFailed: (error: string) => ({ type: AUTH_FAILED, error: error } as AuthFailedAction),

    authCleanError: () => ({ type: AUTH_ERROR_CLEAN } as AuthCleanErrorAction),

    authCleanReg: () => ({ type: AUTH_REGISTERED_CLEAN } as AuthCleanRegAction),*/
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: LeaderboardState = {
    error: '',
    isNewRegistered: false
};

export const leaderboardReducer: Reducer<LeaderboardState> = (state: LeaderboardState = initialState, incomingAction: Action): LeaderboardState => {
    /*const action = incomingAction as KnownAction;
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                error: '',
                isNewRegistered: false
            };
        case AUTH_REG_SUCCESS:
            return {
                error: '',
                isNewRegistered: true
            };
        case AUTH_FAILED:
            return {
                error: action.error,
                isNewRegistered: state.isNewRegistered
            };
        case AUTH_ERROR_CLEAN:
            return {
                error: '',
                isNewRegistered: state.isNewRegistered
            };
        case AUTH_REGISTERED_CLEAN:
            return {
                error: state.error,
                isNewRegistered: false
            };
    }*/

    return state;
};