"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes/routes");
function setSessionItems(auth_token, id, username) {
    sessionStorage.setItem('auth_token', auth_token);
    sessionStorage.setItem('id', id);
    sessionStorage.setItem('username', username);
}
exports.actionCreators = {
    requestAuth: function (username, pwd) { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        fetch(appconfig_1.GATEWAY_ADDR + "/api/auth/", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: pwd
            })
        })
            .then(function (response) {
            if (response.ok)
                return response.json();
            else
                throw response.json();
        })
            .then(function (data) {
            setSessionItems(data.auth_token, data.id, username);
            dispatch(LoaderReducer_1.actionCreators.response());
            dispatch(exports.actionCreators.moveToMainMenu());
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
    requestReg: function (username, pwd) { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        fetch(appconfig_1.GATEWAY_ADDR + "/api/auth/reg/", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: pwd
            })
        })
            .then(function (response) {
            if (response.ok)
                return response.json();
            else
                throw response.json();
        })
            .then(function (data) {
            setSessionItems(data.auth_token, data.id, username);
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
    moveToMainMenu: function () { return function (dispatch) { dispatch(connected_react_router_1.push(Path.MAIN_MENU)); }; },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var initialState = {
    error: '',
    isNewRegistered: false
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