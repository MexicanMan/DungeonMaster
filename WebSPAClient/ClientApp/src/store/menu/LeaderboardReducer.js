"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var actionTypes_1 = require("../actionTypes");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes/routes");
var PAGE_SIZE = 10;
exports.actionCreators = {
    requestLeaderboardPage: function (page) { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        fetch("" + appconfig_1.GATEWAY_ADDR + appconfig_1.GAME_CONTROL_ADDR + "/leaderboard?page=" + page, {
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
            console.log(data);
            if (data.players.length > 0)
                dispatch({
                    type: actionTypes_1.LEADERBOARD_UPDATE_PAGE, currentPage: page,
                    isPrev: page == 0 ? false : true,
                    isNext: data.players.length == PAGE_SIZE ? true : false,
                    players: data.players
                });
            else
                dispatch({
                    type: actionTypes_1.LEADERBOARD_UPDATE_PAGE, currentPage: page,
                    isPrev: page == 0 ? false : true, isNext: false,
                    players: []
                });
            dispatch(LoaderReducer_1.actionCreators.response());
        })
            .catch(function (error) {
            console.log(error);
        });
    }; },
    moveToMainMenu: function () { return function (dispatch) { dispatch(connected_react_router_1.push(Path.MAIN_MENU)); }; },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var initialState = {
    currentPage: 0,
    isPrev: false,
    isNext: true,
    players: []
};
exports.leaderboardReducer = function (state, incomingAction) {
    if (state === void 0) { state = initialState; }
    var action = incomingAction;
    switch (action.type) {
        case actionTypes_1.LEADERBOARD_UPDATE_PAGE:
            return {
                currentPage: action.currentPage,
                isPrev: action.isPrev,
                isNext: action.isNext,
                players: action.players
            };
    }
    return state;
};
//# sourceMappingURL=LeaderboardReducer.js.map