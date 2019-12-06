import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR } from '../../appconfig';
import { AUTH_SUCCESS, AUTH_ERROR_CLEAN, AUTH_REGISTERED_CLEAN, AUTH_FAILED, AUTH_REG_SUCCESS } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AuthState {
    error: string;
    isNewRegistered: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface AuthSuccessAction {
    type: typeof AUTH_SUCCESS;
}

interface AuthFailedAction {
    type: typeof AUTH_FAILED;
    error: string;
}

interface AuthRegSuccessAction {
    type: typeof AUTH_REG_SUCCESS;
}

interface AuthCleanErrorAction {
    type: typeof AUTH_ERROR_CLEAN;
}

interface AuthCleanRegAction {
    type: typeof AUTH_REGISTERED_CLEAN;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = AuthSuccessAction | AuthCleanErrorAction | AuthCleanRegAction | AuthFailedAction | AuthRegSuccessAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

interface AuthResponse {
    id: string;
    auth_token: string;
    expires_in: number;
}

interface OAuthResponse {
    access_token: string;
    refresh_token: string;
    expires_in: number;
}

function setSessionItems(auth_token: string, id: string, username: string) {
    sessionStorage.setItem('auth_token', auth_token);
    sessionStorage.setItem('id', id);
    sessionStorage.setItem('username', username);
    sessionStorage.setItem('scheme', "MicroAuth");
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
                return response.json() as Promise<AuthResponse>;
            else
                throw(response.json() as Promise<ErrorResponse>);
        })
        .then(data => {
            setSessionItems(data.auth_token, data.id, username);

            dispatch({ type: AUTH_SUCCESS });
            dispatch(loaderActionCreators.response());
            dispatch(actionCreators.moveToMainMenu());
        })
        .catch((error: Promise<ErrorResponse>) => {
            error.then(error => {
                dispatch(actionCreators.authFailed(error.error))
                dispatch(loaderActionCreators.response())
            });
        })
        .catch(error => {
            console.log(error);
        });
    },

    requestOAuth: (): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        window.location.href = 'http://localhost:5010/connect/authorize?client_id=spa&scope=openid profile api1 offline_access&response_type=code&redirect_uri=https://localhost:8081/oacallback';

        dispatch(loaderActionCreators.response());
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
                return response.json() as Promise<AuthResponse>;
            else
                throw (response.json() as Promise<ErrorResponse>);
        })
        .then(data => {
            setSessionItems(data.auth_token, data.id, username);

            dispatch({ type: AUTH_REG_SUCCESS });
            dispatch(loaderActionCreators.response());
        })
        .catch((error: Promise<ErrorResponse>) => {
            error.then(error => {
                dispatch(actionCreators.authFailed(error.error))
                dispatch(loaderActionCreators.response())
            });
        })
        .catch(error => {
            console.log(error);
        });
    },

    refreshOAuth: () => {
        let refresh_token : string = sessionStorage.getItem('refresh_token') != null ? sessionStorage.getItem('refresh_token')! : "";

        var details: { [key: string]: string } = {
            'client_id': 'spa',
            'client_secret': 'secret',
            'grant_type': 'refresh_token',
            'refresh_token': refresh_token,
            'redirect_uri': 'https://localhost:8081/oacallback'
        };

        var formBody = [];
        for (var property in details) {
            var encodedKey = encodeURIComponent(property);
            var encodedValue = encodeURIComponent(details[property] as string);
            formBody.push(encodedKey + "=" + encodedValue);
        }
        var formBodyString = formBody.join("&");

        fetch(`http://localhost:5010/connect/token`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: formBodyString
        })
            .then(response => {
                return response.json() as Promise<OAuthResponse>;
            })
            .then(data => {
                sessionStorage.setItem('auth_token', data.access_token);
                sessionStorage.setItem('refresh_token', data.refresh_token);
                sessionStorage.setItem('expires_in', data.expires_in.toString());
                sessionStorage.setItem('scheme', "Bearer");
                sessionStorage.setItem('username', "a");

                setTimeout(function() { actionCreators.refreshOAuth(); }, (data.expires_in-10) * 1000);
            })
            .catch((error: any) => {
                console.log(error);
            });
    },

    moveToMainMenu: () => (dispatch: Dispatch) => { dispatch(push(Path.MAIN_MENU)); },

    authFailed: (error: string) => ({ type: AUTH_FAILED, error: error } as AuthFailedAction),

    authCleanError: () => ({ type: AUTH_ERROR_CLEAN } as AuthCleanErrorAction),

    authCleanReg: () => ({ type: AUTH_REGISTERED_CLEAN } as AuthCleanRegAction),
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: AuthState = {
    error: '',
    isNewRegistered: false,
};

export const authReducer: Reducer<AuthState> = (state: AuthState = initialState, incomingAction: Action): AuthState => {
    const action = incomingAction as KnownAction;
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
    }

    return state;
};