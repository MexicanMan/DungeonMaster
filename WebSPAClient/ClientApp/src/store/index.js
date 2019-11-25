"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Auth = require("./menu/AuthReducer");
var Leaderboard = require("./menu/LeaderboardReducer");
var Loader = require("./helpers/LoaderReducer");
var Field = require("./game/FieldReducer");
var Controller = require("./game/ControllerReducer");
var Log = require("./game/LogReducer");
// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
exports.reducers = {
    auth: Auth.authReducer,
    loader: Loader.loaderReducer,
    field: Field.fieldReducer,
    controller: Controller.controllerReducer,
    leaderboard: Leaderboard.leaderboardReducer,
    log: Log.logReducer
};
//# sourceMappingURL=index.js.map