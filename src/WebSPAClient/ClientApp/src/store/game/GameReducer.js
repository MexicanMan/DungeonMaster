"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var FieldReducer_1 = require("./FieldReducer");
var ControllerReducer_1 = require("./ControllerReducer");
var LogReducer_1 = require("./LogReducer");
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
function getCurrentDate() {
    var now = new Date();
    var stringDate = now.getFullYear().toString() + "-" + now.getMonth().toString() + "-" + now.getDate().toString() +
        " " + now.getHours().toString() + ":" + now.getMinutes().toString();
    return stringDate;
}
function actionLogBuilder(action) {
    var actionString = "• " + getCurrentDate() + " " + action;
    return actionString;
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
            dispatch(LogReducer_1.actionCreators.logUpdate(actionLogBuilder("You entered the game! Now you are at the room #" + data.room.roomId + ".")));
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
    gamePatchRequest: function (action, actionLogString, bodyDirection) { return function (dispatch) {
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
            if (bodyDirection == undefined)
                dispatch(LogReducer_1.actionCreators.logUpdate(actionLogBuilder(actionLogString)));
            else
                dispatch(LogReducer_1.actionCreators.logUpdate(actionLogBuilder(actionLogString + data.room.roomId)));
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
        dispatch(LogReducer_1.actionCreators.cleanState());
    }; }
};
//# sourceMappingURL=GameReducer.js.map