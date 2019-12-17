import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { LOG_CLEAN, LOG_UPDATE } from '../actionTypes';

const LOG_SIZE = 20;

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LogState {
    log: string[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

export interface LogUpdateAction {
    type: typeof LOG_UPDATE;
    actionLog: string;
}

export interface LogCleanAction {
    type: typeof LOG_CLEAN;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = LogUpdateAction | LogCleanAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    logUpdate: (actionString: string) => (dispatch: Dispatch) => {
        dispatch({
            type: LOG_UPDATE, actionLog: actionString
        });
    },

    cleanState: () => ({ type: LOG_CLEAN } as LogCleanAction),
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const initialState: LogState = {
    log: [],
};

export const logReducer: Reducer<LogState> = (state: LogState = initialState, incomingAction: Action): LogState => {
    const knownAction = incomingAction as KnownAction;
    switch (knownAction.type) {
        case LOG_UPDATE: {
            const action = knownAction as LogUpdateAction;

            let newLog: string[] = [];
            for (var i = 0; i < state.log.length; i++) {
                newLog.push(state.log[i]);
            }

            if (state.log.length > LOG_SIZE)
                newLog.shift();
            newLog.push(action.actionLog);

            return {
                log: newLog
            };
        }
        case LOG_CLEAN:
            return initialState;
    }

    return state;
};