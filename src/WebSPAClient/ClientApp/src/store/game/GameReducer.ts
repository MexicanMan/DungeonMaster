import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR, GAME_CONTROL_ADDR } from '../../appconfig';
import {  } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { actionCreators as fieldActionCreators } from './FieldReducer';
import { actionCreators as controllerActionCreators } from './ControllerReducer';
import { actionCreators as logActionCreators } from './LogReducer';
import { MonsterTypes } from '../helpers/MonsterTypes';
import { Directions } from '../helpers/Directions';

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

function getCurrentDate(): string {
    let now = new Date();

    let stringDate = now.getFullYear().toString() + "-" + now.getMonth().toString() + "-" + now.getDate().toString() +
        " " + now.getHours().toString() + ":" + now.getMinutes().toString();

    return stringDate;
}

function actionLogBuilder(action: string): string {
    let actionString = "• " + getCurrentDate() + " " + action;

    return actionString;
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

                dispatch(logActionCreators.logUpdate(actionLogBuilder(`You entered the game! Now you are at the room #${data.room.roomId}.`)));
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

    gamePatchRequest: (action: string, actionLogString: string, bodyDirection?: Directions): AppThunkAction<any> => (dispatch) => {
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

                if (bodyDirection == undefined)
                    dispatch(logActionCreators.logUpdate(actionLogBuilder(actionLogString)));
                else
                    dispatch(logActionCreators.logUpdate(actionLogBuilder(actionLogString + data.room.roomId)));
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
        dispatch(logActionCreators.cleanState());
    }
};