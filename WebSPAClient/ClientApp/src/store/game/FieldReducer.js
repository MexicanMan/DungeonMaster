"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var actionTypes_1 = require("../actionTypes");
var RoomSymbolTypes = require("../helpers/RoomSymbolTypes");
var Directions_1 = require("../helpers/Directions");
var ROOM_SIZE = 5;
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    roomUpdate: function (treasure, northRoom, southRoom, eastRoom, westRoom, northRoomId, southRoomId, eastRoomId, westRoomId, monsterId, playerTo) { return function (dispatch) {
        var northRoomSymb = northRoom ?
            northRoomId == undefined ? RoomSymbolTypes.NORTH_CLOSED : RoomSymbolTypes.NORTH_OPENED :
            RoomSymbolTypes.WALL;
        var eastRoomSymb = eastRoom ?
            eastRoomId == undefined ? RoomSymbolTypes.EAST_CLOSED : RoomSymbolTypes.EAST_OPENED :
            RoomSymbolTypes.WALL;
        var westRoomSymb = westRoom ?
            westRoomId == undefined ? RoomSymbolTypes.WEST_CLOSED : RoomSymbolTypes.WEST_OPENED :
            RoomSymbolTypes.WALL;
        var southRoomSymb = southRoom ?
            southRoomId == undefined ? RoomSymbolTypes.SOUTH_CLOSED : RoomSymbolTypes.SOUTH_OPENED :
            RoomSymbolTypes.WALL;
        var treasureSymb = treasure ? RoomSymbolTypes.TREASURE : RoomSymbolTypes.EMPTY;
        var monsterSymb = monsterId == undefined ? RoomSymbolTypes.EMPTY : RoomSymbolTypes.MONSTER;
        var room = [
            [RoomSymbolTypes.WALL, RoomSymbolTypes.WALL, northRoomSymb, RoomSymbolTypes.WALL, RoomSymbolTypes.WALL],
            [RoomSymbolTypes.WALL, treasureSymb, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.WALL],
            [westRoomSymb, RoomSymbolTypes.EMPTY, monsterSymb, RoomSymbolTypes.EMPTY, eastRoomSymb],
            [RoomSymbolTypes.WALL, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.EMPTY, RoomSymbolTypes.WALL],
            [RoomSymbolTypes.WALL, RoomSymbolTypes.WALL, southRoomSymb, RoomSymbolTypes.WALL, RoomSymbolTypes.WALL]
        ];
        var playerCoords = [3, 2];
        if (playerTo != undefined) {
            switch (playerTo) {
                case Directions_1.Directions.East:
                    playerCoords = [2, 1];
                    break;
                case Directions_1.Directions.West:
                    playerCoords = [2, 3];
                    break;
                case Directions_1.Directions.South:
                    playerCoords = [1, 2];
                    break;
            }
        }
        room[playerCoords[0]][playerCoords[1]] = RoomSymbolTypes.PLAYER;
        dispatch({ type: actionTypes_1.ROOM_UPDATE, room: room });
    }; },
    playerUpdate: function (currentHP, maxHP) { return function (dispatch) {
        dispatch({ type: actionTypes_1.PLAYER_UPDATE, maxHP: maxHP, HP: currentHP });
    }; },
    monsterUpdate: function (currentHP, maxHP, type) { return function (dispatch) {
        dispatch({ type: actionTypes_1.MONSTER_UPDATE, maxHP: maxHP, HP: currentHP, monsterType: type });
    }; },
    cleanState: function () { return ({ type: actionTypes_1.FIELD_CLEAN }); },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
exports.initialState = {
    room: [],
    playerMaxHP: 3,
    playerHP: 3,
};
exports.fieldReducer = function (state, incomingAction) {
    if (state === void 0) { state = exports.initialState; }
    var knownAction = incomingAction;
    switch (knownAction.type) {
        case actionTypes_1.ROOM_UPDATE: {
            var action = knownAction;
            return __assign(__assign({}, state), { room: action.room });
        }
        case actionTypes_1.PLAYER_UPDATE: {
            var action = knownAction;
            return __assign(__assign({}, state), { playerMaxHP: action.maxHP, playerHP: action.HP });
        }
        case actionTypes_1.MONSTER_UPDATE: {
            var action = knownAction;
            return __assign(__assign({}, state), { monsterMaxHP: action.maxHP, monsterHP: action.HP, monsterType: action.monsterType });
        }
        case actionTypes_1.FIELD_CLEAN: {
            return exports.initialState;
        }
    }
    return state;
};
//# sourceMappingURL=FieldReducer.js.map