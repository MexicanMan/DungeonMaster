"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var actionTypes_1 = require("../actionTypes");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes");
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
            sessionStorage.setItem('auth_token', data.auth_token);
            sessionStorage.setItem('id', data.id);
            dispatch({ type: actionTypes_1.AUTH_SUCCESS });
            dispatch(LoaderReducer_1.actionCreators.response());
            dispatch(exports.actionCreators.moveToMainMenu());
        })
            .catch(function (error) {
            error.then(function (error) {
                dispatch(exports.actionCreators.authFailed(error.error));
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
            sessionStorage.setItem('auth_token', data.auth_token);
            sessionStorage.setItem('id', data.id);
            dispatch({ type: actionTypes_1.AUTH_REG_SUCCESS });
            dispatch(LoaderReducer_1.actionCreators.response());
        })
            .catch(function (error) {
            error.then(function (error) {
                dispatch(exports.actionCreators.authFailed(error.error));
                dispatch(LoaderReducer_1.actionCreators.response());
            });
        })
            .catch(function (error) {
            console.log(error);
        });
    }; },
    moveToMainMenu: function () { return function (dispatch) { dispatch(connected_react_router_1.push(Path.MAIN_MENU)); }; },
    authFailed: function (error) { return ({ type: actionTypes_1.AUTH_FAILED, error: error }); },
    authCleanError: function () { return ({ type: actionTypes_1.AUTH_ERROR_CLEAN }); },
    authCleanReg: function () { return ({ type: actionTypes_1.AUTH_REGISTERED_CLEAN }); },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var initialState = {
    error: '',
    isNewRegistered: false,
};
exports.authReducer = function (state, incomingAction) {
    if (state === void 0) { state = initialState; }
    var action = incomingAction;
    switch (action.type) {
        case actionTypes_1.AUTH_SUCCESS:
            return {
                error: '',
                isNewRegistered: false
            };
        case actionTypes_1.AUTH_REG_SUCCESS:
            return {
                error: '',
                isNewRegistered: true
            };
        case actionTypes_1.AUTH_FAILED:
            return {
                error: action.error,
                isNewRegistered: state.isNewRegistered
            };
        case actionTypes_1.AUTH_ERROR_CLEAN:
            return {
                error: '',
                isNewRegistered: state.isNewRegistered
            };
        case actionTypes_1.AUTH_REGISTERED_CLEAN:
            return {
                error: state.error,
                isNewRegistered: false
            };
    }
    return state;
};
//# sourceMappingURL=AuthReducer.js.map