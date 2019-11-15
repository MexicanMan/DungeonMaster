import { Action, Reducer } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR } from '../../appconfig';
import { AUTH_SUCCESS } from '../actionTypes';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AuthState {
    error: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface AuthSuccessAction {
    type: typeof AUTH_SUCCESS;
}

interface AuthRequestAction {
    type: "AUTH_REQ";
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = AuthSuccessAction | AuthRequestAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

interface AuthResponse {
    id: string;
    auth_token: string;
    expires_in: number;
}

export const actionCreators = {
    requestAuth: (username: string, pwd: string): AppThunkAction<KnownAction> => (dispatch) => {
        dispatch({ type: 'AUTH_REQ' });

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
            .then(response => response.json() as Promise<AuthResponse>)
            .then(data => {
            sessionStorage.setItem('auth_token', data.auth_token);
            sessionStorage.setItem('id', data.id);
            dispatch({ type: AUTH_SUCCESS });
        });
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: AuthState = {
    error: ''
};

export const authReducer: Reducer<AuthState> = (state: AuthState = initialState, incomingAction: Action): AuthState => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'AUTH_SUCCESS':
            return {
                error: ''
            };
    }

    return state;
};