"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var actionTypes_1 = require("../actionTypes");
var LoaderReducer_1 = require("../helpers/LoaderReducer");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes/routes");
function setSessionItems(auth_token, id, username) {
    sessionStorage.setItem('auth_token', auth_token);
    sessionStorage.setItem('id', id);
    sessionStorage.setItem('username', username);
    sessionStorage.setItem('scheme', "MicroAuth");
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
    requestOAuth: function () { return function (dispatch) {
        dispatch(LoaderReducer_1.actionCreators.request());
        window.location.href = 'http://localhost:5010/connect/authorize?client_id=spa&scope=openid profile api1 offline_access&response_type=code&redirect_uri=https://localhost:8081/oacallback';
        dispatch(LoaderReducer_1.actionCreators.response());
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
    refreshOAuth: function () {
        var refresh_token = sessionStorage.getItem('refresh_token') != null ? sessionStorage.getItem('refresh_token') : "";
        var details = {
            'client_id': 'spa',
            'client_secret': 'secret',
            'grant_type': 'refresh_token',
            'refresh_token': refresh_token,
            'redirect_uri': 'https://localhost:8081/oacallback'
        };
        var formBody = [];
        for (var property in details) {
            var encodedKey = encodeURIComponent(property);
            var encodedValue = encodeURIComponent(details[property]);
            formBody.push(encodedKey + "=" + encodedValue);
        }
        var formBodyString = formBody.join("&");
        fetch("http://localhost:5010/connect/token", {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: formBodyString
        })
            .then(function (response) {
            return response.json();
        })
            .then(function (data) {
            sessionStorage.setItem('auth_token', data.access_token);
            sessionStorage.setItem('refresh_token', data.refresh_token);
            sessionStorage.setItem('expires_in', data.expires_in.toString());
            sessionStorage.setItem('scheme', "Bearer");
            sessionStorage.setItem('username', "a");
            setTimeout(function () { exports.actionCreators.refreshOAuth(); }, (data.expires_in - 10) * 1000);
        })
            .catch(function (error) {
            console.log(error);
        });
    },
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