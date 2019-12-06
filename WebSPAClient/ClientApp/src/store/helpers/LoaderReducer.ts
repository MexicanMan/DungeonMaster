import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { REQUEST, REQUEST_RESPONSE } from '../actionTypes';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';
import { actionCreators as authActionCreators } from '../menu/AuthReducer';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LoaderState {
    loading: boolean;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAction {
    type: typeof REQUEST;
}

interface ResponseAction {
    type: typeof REQUEST_RESPONSE;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAction | ResponseAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

interface OAuthResponse {
    access_token: string;
    refresh_token: string;
    expires_in: number;
}

export const actionCreators = {
    request: () => ({ type: REQUEST } as RequestAction),
    response: () => ({ type: REQUEST_RESPONSE } as ResponseAction),

    requestOAuth: (code: string): AppThunkAction<any> => (dispatch) => {
        dispatch(actionCreators.request());

        var details: {[key: string] : string} = {
            'client_id': 'spa',
            'client_secret': 'secret',
            'grant_type': 'authorization_code',
            'code': code,
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
                console.log(data);
                sessionStorage.setItem('auth_token', data.access_token);
                sessionStorage.setItem('refresh_token', data.refresh_token);
                sessionStorage.setItem('expires_in', data.expires_in.toString());
                sessionStorage.setItem('scheme', "Bearer");
                sessionStorage.setItem('username', "a");
                
                let timeout = (data.expires_in - 10) * 1000;
                setTimeout(function() { authActionCreators.refreshOAuth(); }, timeout);

                dispatch(actionCreators.response());
                dispatch(actionCreators.moveToMainMenu());
            })
            .catch((error: any) => {
                error.then(() => {
                    dispatch(actionCreators.response())
                });
            })
            .catch(error => {
                console.log(error);
            });
    },

    moveToMainMenu: () => (dispatch: Dispatch) => { dispatch(push(Path.MAIN_MENU)); },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: LoaderState = {
    loading: false
};

export const loaderReducer: Reducer<LoaderState> = (state: LoaderState = initialState, incomingAction: Action): LoaderState => {
    const action = incomingAction as KnownAction;

    switch (action.type) {
        case REQUEST:
            return {
                loading: true
            };
        case REQUEST_RESPONSE:
            return {
                loading: false
            };
    }

    return state;
};