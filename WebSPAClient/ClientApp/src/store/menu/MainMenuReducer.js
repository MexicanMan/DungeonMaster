"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes");
exports.actionCreators = {
    moveToGame: function () { return function (dispatch) { dispatch(connected_react_router_1.push(Path.MAIN_MENU)); }; },
};
//# sourceMappingURL=MainMenuReducer.js.map