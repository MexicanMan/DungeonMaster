import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR, GAME_CONTROL_ADDR } from '../../appconfig';
import {  } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';
import { FieldState, actionCreators as fieldActionCreators, initialState as FieldInitialState } from './FieldReducer';
import { actionCreators as controllerActionCreators } from './ControllerReducer';
import { MonsterTypes } from '../helpers/MonsterTypes';
import { Directions } from '../helpers/Directions';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface GameState {
    field: FieldState | undefined,
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

interface GameDataResponse {
    room: Room,
    player: Player,
    monster: Monster
}

interface Room {
    roomId: number,
    monsterId?: number,
    treasure: boolean
    northRoom: boolean,
    southRoom: boolean,
    eastRoom: boolean,
    westRoom: boolean,
    northRoomId?: number,
    southRoomId?: number,
    eastRoomId?: number,
    westRoomId?: number,
    discovererId: string
}

interface Player {
    id: string,
    username: string,
    currentRoomId?: number,
    maxHP: number,
    currentHP: number,
    treasureCount: number
}

interface Monster {
    monsterId: number,
    maxHP: number,
    currentHP: number,
    type: MonsterTypes
}

function GameDataUpdate(dispatch: (action: any) => void, data: GameDataResponse, playerTo?: Directions) {
    let room = data.room;
    dispatch(fieldActionCreators.roomUpdate(room.treasure, room.northRoom, room.southRoom, room.eastRoom, room.westRoom,
        room.northRoomId, room.southRoomId, room.eastRoomId, room.westRoomId, room.monsterId, playerTo));
    dispatch(controllerActionCreators.controllerUpdate(room.treasure, room.northRoom, room.southRoom, room.eastRoom,
        room.westRoom, room.monsterId));

    let player = data.player;
    dispatch(fieldActionCreators.playerUpdate(player.currentHP, player.maxHP));

    let monster = data.monster;
    if (monster != undefined)
        dispatch(fieldActionCreators.monsterUpdate(monster.currentHP, monster.maxHP, monster.type));
    else
        dispatch(fieldActionCreators.monsterUpdate(undefined, undefined, undefined));
}

export const actionCreators = {
    requestEnterGame: (): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        fetch(`${GATEWAY_ADDR}${GAME_CONTROL_ADDR}/room`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem("auth_token")}`
            }
        })
             .then(response => {
                if (response.ok)
                    return response.json() as Promise<GameDataResponse>;
                else
                    throw (response.json() as Promise<ErrorResponse>);
            })
            .then(data => {
                GameDataUpdate(dispatch, data);

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

    gamePatchRequest: (action: string, bodyDirection?: Directions): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        fetch(`${GATEWAY_ADDR}${GAME_CONTROL_ADDR}/${action}`, {
            method: 'PATCH',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem("auth_token")}`
            },
            body: bodyDirection == undefined ? null : JSON.stringify({
                toDir: bodyDirection
            })
        })
            .then(response => {
                if (response.ok)
                    return response.json() as Promise<GameDataResponse>;
                else
                    throw (response.json() as Promise<ErrorResponse>);
            })
            .then(data => {
                GameDataUpdate(dispatch, data, bodyDirection);

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

    cleanGameStates: () => (dispatch: Dispatch) => {
        dispatch(fieldActionCreators.cleanState());
        dispatch(controllerActionCreators.cleanState());
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: GameState = {
    field: FieldInitialState,
};

export const gameReducer: Reducer<GameState> = (state: GameState = initialState, incomingAction: Action): GameState => {
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