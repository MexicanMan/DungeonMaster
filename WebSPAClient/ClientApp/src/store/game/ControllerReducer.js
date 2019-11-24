"use strict";
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
    monsterAtk: function () { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('monster')); }; },
    treasurePkp: function () { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('treasure')); }; },
    moveToRoom: function (toDir) { return function (dispatch) { dispatch(GameReducer_1.actionCreators.gamePatchRequest('room', toDir)); }; },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
exports.initialState = {
    isLeftActive: false,
    isRightActive: false,
    isDownActive: false,
    isUpActive: false,
    isMonsterActive: false,
    isTreasureActive: false
};
exports.controllerReducer = function (state, incomingAction) {
    if (state === void 0) { state = exports.initialState; }
    var knownAction = incomingAction;
    switch (knownAction.type) {
        case actionTypes_1.CONTROLLER_UPDATE: {
            var action = knownAction;
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
        case actionTypes_1.CONTROLLER_CLEAN:
            return exports.initialState;
    }
    return state;
};
//# sourceMappingURL=ControllerReducer.js.map