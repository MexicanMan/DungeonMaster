import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR, GAME_CONTROL_ADDR } from '../../appconfig';
import { ROOM_UPDATE, PLAYER_UPDATE, MONSTER_UPDATE, FIELD_CLEAN } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';
import { MonsterTypes } from '../helpers/MonsterTypes';
import * as RoomSymbolTypes from '../helpers/RoomSymbolTypes';
import { Directions } from '../helpers/Directions';

const ROOM_SIZE: number = 5;

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface FieldState {
    room: number[][],
    playerMaxHP: number,
    playerHP: number,
    monsterMaxHP?: number,
    monsterHP?: number,
    monsterType?: MonsterTypes
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

export interface RoomUpdateAction {
    type: typeof ROOM_UPDATE;
    room: number[][];
}

export interface PlayerUpdateAction {
    type: typeof PLAYER_UPDATE;
    maxHP: number;
    HP: number;
}

export interface MonsterUpdateAction {
    type: typeof MONSTER_UPDATE;
    maxHP: number;
    HP: number;
    monsterType: MonsterTypes;
}

export interface FieldCleanAction {
    type: typeof FIELD_CLEAN;
}


// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
export type KnownAction = RoomUpdateAction | PlayerUpdateAction | MonsterUpdateAction | FieldCleanAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    roomUpdate: (treasure: boolean, northRoom: boolean, southRoom: boolean,
        eastRoom: boolean, westRoom: boolean, northRoomId?: number, southRoomId?: number,
        eastRoomId?: number, westRoomId?: number, monsterId?: number, playerTo?: Directions) => (dispatch: Dispatch) => {
            let northRoomSymb = northRoom ?
                northRoomId == undefined ? RoomSymbolTypes.NORTH_CLOSED : RoomSymbolTypes.NORTH_OPENED :
                RoomSymbolTypes.WALL;
            let eastRoomSymb = eastRoom ?
                eastRoomId == undefined ? RoomSymbolTypes.EAST_CLOSED : RoomSymbolTypes.EAST_OPENED :
                RoomSymbolTypes.WALL;
            let westRoomSymb = westRoom ?
                westRoomId == undefined ? RoomSymbolTypes.WEST_CLOSED : RoomSymbolTypes.WEST_OPENED :
                RoomSymbolTypes.WALL;
            let southRoomSymb = southRoom ?
                southRoomId == undefined ? RoomSymbolTypes.SOUTH_CLOSED : RoomSymbolTypes.SOUTH_OPENED :
                RoomSymbolTypes.WALL;
            let treasureSymb = treasure ? RoomSymbolTypes.TREASURE : RoomSymbolTypes.EMPTY;
            let monsterSymb = monsterId == undefined ? RoomSymbolTypes.EMPTY : RoomSymbolTypes.MONSTER;

            let room: number[][] = [
                [RoomSymbolTypes.WALL, RoomSymbolTypes.WALL, northRoomSymb, RoomSymbolTypes.WALL, RoomSymbolTypes.WALL],
                [RoomSymbolTypes.WALL, treasureSymb, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.WALL],
                [westRoomSymb, RoomSymbolTypes.EMPTY, monsterSymb, RoomSymbolTypes.EMPTY, eastRoomSymb],
                [RoomSymbolTypes.WALL, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.WALL],
                [RoomSymbolTypes.WALL, RoomSymbolTypes.WALL, southRoomSymb, RoomSymbolTypes.WALL, RoomSymbolTypes.WALL]
            ]

            let playerCoords = [3, 2];
            if (playerTo != undefined) {
                switch (playerTo) {
                    case Directions.East:
                        playerCoords = [2, 1];
                        break;
                    case Directions.West:
                        playerCoords = [2, 3];
                        break;
                    case Directions.South:
                        playerCoords = [1, 2];
                        break;
                }
            }

            room[playerCoords[0]][playerCoords[1]] = RoomSymbolTypes.PLAYER;
            
            dispatch({ type: ROOM_UPDATE, room: room });
        },

    playerUpdate: (currentHP: number, maxHP: number) => (dispatch: Dispatch) => {
        dispatch({ type: PLAYER_UPDATE, maxHP: maxHP, HP: currentHP });
    },

    monsterUpdate: (currentHP?: number, maxHP?: number, type?: MonsterTypes) => (dispatch: Dispatch) => {
        dispatch({ type: MONSTER_UPDATE, maxHP: maxHP, HP: currentHP, monsterType: type });
    },

    cleanState: () => ({ type: FIELD_CLEAN } as FieldCleanAction),
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

export const initialState: FieldState = {
    room: [],
    playerMaxHP: 3,
    playerHP: 3,
};

export const fieldReducer: Reducer<FieldState> = (state: FieldState = initialState, incomingAction: Action): FieldState => {
    const knownAction = incomingAction as KnownAction;
    switch (knownAction.type) {
        case ROOM_UPDATE: {
            const action = knownAction as RoomUpdateAction;
            return {
                ...state,
                room: action.room,
            };
        }
        case PLAYER_UPDATE: {
            const action = knownAction as PlayerUpdateAction;
            return {
                ...state,
                playerMaxHP: action.maxHP,
                playerHP: action.HP
            }
        }
        case MONSTER_UPDATE: {
            const action = knownAction as MonsterUpdateAction;
            return {
                ...state,
                monsterMaxHP: action.maxHP,
                monsterHP: action.HP,
                monsterType: action.monsterType
            }
        }
        case FIELD_CLEAN: {
            return initialState;
        }
    }

    return state;
};