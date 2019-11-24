"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WeatherForecasts = require("./WeatherForecasts");
var Counter = require("./Counter");
var Auth = require("./menu/AuthReducer");
var Leaderboard = require("./menu/LeaderboardReducer");
var Loader = require("./helpers/LoaderReducer");
var Game = require("./game/GameReducer");
var Field = require("./game/FieldReducer");
var Controller = require("./game/ControllerReducer");
// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
exports.reducers = {
    counter: Counter.reducer,
    weatherForecasts: WeatherForecasts.reducer,
    auth: Auth.authReducer,
    loader: Loader.loaderReducer,
    game: Game.gameReducer,
    field: Field.fieldReducer,
    controller: Controller.controllerReducer,
    leaderboard: Leaderboard.leaderboardReducer
};
//# sourceMappingURL=index.js.map