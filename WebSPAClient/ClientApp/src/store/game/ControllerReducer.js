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
var GameReducer_1 = require("./GameReducer");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes/routes");
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    controllerUpdate: function (treasure, northRoom, southRoom, eastRoom, westRoom, monsterId) { return function (dispatch) {
        var isMonsterActive = monsterId == undefined ? false : true;
        var isTreasureActive = treasure ? monsterId == undefined ? true : false : false;
        dispatch({
            type: actionTypes_1.CONTROLLER_UPDATE, isLeftActive: westRoom, isRightActive: eastRoom,
            isUpActive: northRoom, isDownActive: southRoom, isTreasureActive: isTreasureActive,
            isMonsterActive: isMonsterActive
        });
    }; },
    moveToMenu: function () { return function (dispatch) {
        dispatch(connected_react_router_1.push(Path.MAIN_MENU));
    }; },
    cleanState: function () { return ({ type: actionTypes_1.CONTROLLER_CLEAN }); },
    monsterAtk: function () { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('monster', 'You attacked the monster!')); }; },
    treasurePkp: function () { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('treasure', 'You picked up the treasure!')); }; },
    moveToRoom: function (toDir) { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('room', 'You entered the room #', toDir)); }; },
    changeLeaderboardModal: function (isOpened) { return ({
        type: actionTypes_1.CHANGE_LEADERBOARD_MODAL, isLeaderboardOpened: isOpened
    }); },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
exports.initialState = {
    isLeftActive: false,
    isRightActive: false,
    isDownActive: false,
    isUpActive: false,
    isMonsterActive: false,
    isTreasureActive: false,
    isLeaderboardOpened: false
};
exports.controllerReducer = function (state, incomingAction) {
    if (state === void 0) { state = exports.initialState; }
    var knownAction = incomingAction;
    switch (knownAction.type) {
        case actionTypes_1.CONTROLLER_UPDATE: {
            var action_1 = knownAction;
            return {
                isRightActive: action_1.isRightActive,
                isLeftActive: action_1.isLeftActive,
                isDownActive: action_1.isDownActive,
                isUpActive: action_1.isUpActive,
                isMonsterActive: action_1.isMonsterActive,
                isTreasureActive: action_1.isTreasureActive,
                isLeaderboardOpened: state.isLeaderboardOpened
            };
        }
        case actionTypes_1.CONTROLLER_CLEAN:
            return exports.initialState;
        case actionTypes_1.CHANGE_LEADERBOARD_MODAL:
            var action = knownAction;
            return __assign(__assign({}, state), { isLeaderboardOpened: action.isLeaderboardOpened });
    }
    return state;
};
//# sourceMappingURL=ControllerReducer.js.map