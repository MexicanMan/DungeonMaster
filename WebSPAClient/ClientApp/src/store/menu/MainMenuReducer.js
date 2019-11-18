"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes");
exports.actionCreators = {
    moveToGame: function () { return function (dispatch) {
        dispatch(connected_react_router_1.push(Path.GAME));
    }; },
    moveToLeaderboard: function () { return function (dispatch) {
        dispatch(connected_react_router_1.push(Path.LEADERBOARD));
    }; },
    exit: function () { return function (dispatch) {
        dispatch(connected_react_router_1.push("/"));
    }; },
};
//# sourceMappingURL=MainMenuReducer.js.map