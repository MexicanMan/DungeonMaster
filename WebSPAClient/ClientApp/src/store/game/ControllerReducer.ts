import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR, GAME_CONTROL_ADDR } from '../../appconfig';
import { CONTROLLER_UPDATE, CONTROLLER_CLEAN } from '../actionTypes';
import { actionCreators as gameActionCreators } from './GameReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';
import { Directions } from '../helpers/Directions';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface ControllerState {
    isLeftActive: boolean,
    isRightActive: boolean,
    isUpActive: boolean,
    isDownActive: boolean,
    isTreasureActive: boolean,
    isMonsterActive: boolean
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

export interface ControllerUpdateAction {
    type: typeof CONTROLLER_UPDATE;
    isLeftActive: boolean;
    isRightActive: boolean;
    isUpActive: boolean;
    isDownActive: boolean;
    isTreasureActive: boolean;
    isMonsterActive: boolean;
}

export interface ControllerCleanAction {
    type: typeof CONTROLLER_CLEAN;
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = ControllerUpdateAction | ControllerCleanAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    controllerUpdate: (treasure: boolean, northRoom: boolean, southRoom: boolean,
        eastRoom: boolean, westRoom: boolean, monsterId?: number) => (dispatch: Dispatch) => {
            let isMonsterActive = monsterId == undefined ? false : true;
            let isTreasureActive = treasure ? monsterId == undefined ? true : false : false;
            
            dispatch({
                type: CONTROLLER_UPDATE, isLeftActive: westRoom, isRightActive: eastRoom,
                isUpActive: northRoom, isDownActive: southRoom, isTreasureActive: isTreasureActive,
                isMonsterActive: isMonsterActive
            });
        },

    moveToMenu: () => (dispatch: Dispatch) => {
        dispatch(push(Path.MAIN_MENU));
    },

    cleanState: () => ({ type: CONTROLLER_CLEAN } as ControllerCleanAction),

    monsterAtk: (): AppThunkAction<any> => (dispatch) => { dispatch(gameActionCreators.gamePatchRequest('monster')); },

    treasurePkp: (): AppThunkAction<any> => (dispatch) => { dispatch(gameActionCreators.gamePatchRequest('treasure')); },

    moveToRoom: (toDir: Directions): AppThunkAction<any> => (dispatch) => { dispatch(gameActionCreators.gamePatchRequest('room', toDir)); },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const initialState: ControllerState = {
    isLeftActive: false,
    isRightActive: false,
    isDownActive: false,
    isUpActive: false,
    isMonsterActive: false,
    isTreasureActive: false
};

export const controllerReducer: Reducer<ControllerState> = (state: ControllerState = initialState, incomingAction: Action): ControllerState => {
    const knownAction = incomingAction as KnownAction;
    switch (knownAction.type) {
        case CONTROLLER_UPDATE: {
            const action = knownAction as ControllerUpdateAction;
            console.log(action);
            return {
                isRightActive: action.isRightActive,
                isLeftActive: action.isLeftActive,
                isDownActive: action.isDownActive,
                isUpActive: action.isUpActive,
                isMonsterActive: action.isMonsterActive,
                isTreasureActive: action.isTreasureActive
            };
        }
        case CONTROLLER_CLEAN:
            return initialState;
    }

    return state;
};