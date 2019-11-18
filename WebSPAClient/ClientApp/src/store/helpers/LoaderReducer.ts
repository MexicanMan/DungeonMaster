import { Action, Reducer } from 'redux';
import { AppThunkAction } from '../';
import { REQUEST, REQUEST_RESPONSE } from '../actionTypes';

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

export const actionCreators = {
    request: () => ({ type: REQUEST } as RequestAction),
    response: () => ({ type: REQUEST_RESPONSE } as ResponseAction)
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