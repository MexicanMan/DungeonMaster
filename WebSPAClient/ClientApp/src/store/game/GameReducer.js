"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var FieldReducer_1 = require("./FieldReducer");
var ControllerReducer_1 = require("./ControllerReducer");
function GameDataUpdate(dispatch, data, playerTo) {
    var room = data.room;
    dispatch(FieldReducer_1.actionCreators.roomUpdate(room.treasure, room.northRoom, room.southRoom, room.eastRoom, room.westRoom, room.northRoomId, room.southRoomId, room.eastRoomId, room.westRoomId, room.monsterId, playerTo));
    dispatch(ControllerReducer_1.actionCreators.controllerUpdate(room.treasure, room.northRoom, room.southRoom, room.eastRoom, room.westRoom, room.monsterId));
    var player = data.player;
    dispatch(FieldReducer_1.actionCreators.playerUpdate(player.currentHP, player.maxHP));
    var monster = data.monster;
    if (monster != undefined)
        dispatch(FieldReducer_1.actionCreators.monsterUpdate(monster.currentHP, monster.maxHP, monster.type));
    else
        dispatch(FieldReducer_1.actionCreators.monsterUpdate(undefined, undefined, undefined));
}
exports.actionCreators = {
    requestEnterGame: function () { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        fetch("" + appconfig_1.GATEWAY_ADDR + appconfig_1.GAME_CONTROL_ADDR + "/room", {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Authorization': "Bearer " + sessionStorage.getItem("auth_token")
            }
        })
            .then(function (response) {
            if (response.ok)
                return response.json();
            else
                throw response.json();
        })
            .then(function (data) {
            GameDataUpdate(dispatch, data);
            dispatch(LoaderReducer_1.actionCreators.response());
        })
            .catch(function (error) {
            error.then(function (error) {
                dispatch(LoaderReducer_1.actionCreators.response());
            });
        })
            .catch(function (error) {
            console.log(error);
        });
    }; },
    gamePatchRequest: function (action, bodyDirection) { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        fetch("" + appconfig_1.GATEWAY_ADDR + appconfig_1.GAME_CONTROL_ADDR + "/" + action, {
            method: 'PATCH',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': "Bearer " + sessionStorage.getItem("auth_token")
            },
            body: bodyDirection == undefined ? null : JSON.stringify({
                toDir: bodyDirection
            })
        })
            .then(function (response) {
            if (response.ok)
                return response.json();
            else
                throw response.json();
        })
            .then(function (data) {
            GameDataUpdate(dispatch, data, bodyDirection);
            dispatch(LoaderReducer_1.actionCreators.response());
        })
            .catch(function (error) {
            error.then(function (error) {
                dispatch(LoaderReducer_1.actionCreators.response());
            });
        })
            .catch(function (error) {
            console.log(error);
        });
    }; },
    cleanGameStates: function () { return function (dispatch) {
        dispatch(FieldReducer_1.actionCreators.cleanState());
        dispatch(ControllerReducer_1.actionCreators.cleanState());
    }; }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var initialState = {
    field: FieldReducer_1.initialState,
};
exports.gameReducer = function (state, incomingAction) {
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
    if (state === void 0) { state = initialState; }
    return state;
};
//# sourceMappingURL=GameReducer.js.map