"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var actionTypes_1 = require("../actionTypes");
var connected_react_router_1 = require("connected-react-router");
var Path = require("../../routes/routes");
exports.actionCreators = {
    request: function () { return ({ type: actionTypes_1.REQUEST }); },
    response: function () { return ({ type: actionTypes_1.REQUEST_RESPONSE }); },
    requestOAuth: function (code) { return function (dispatch) {
        dispatch(exports.actionCreators.request());
        var details = {
            'client_id': 'spa',
            'client_secret': 'secret',
            'grant_type': 'authorization_code',
            'code': code,
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
            console.log(data);
            sessionStorage.setItem('auth_token', data.access_token);
            sessionStorage.setItem('expires_in', data.expires_in.toString());
            sessionStorage.setItem('scheme', "Bearer");
            sessionStorage.setItem('username', "player");
            dispatch(exports.actionCreators.response());
            dispatch(exports.actionCreators.moveToMainMenu());
        })
            .catch(function (error) {
            error.then(function () {
                dispatch(exports.actionCreators.response());
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
    loading: false
};
exports.loaderReducer = function (state, incomingAction) {
    if (state === void 0) { state = initialState; }
    var action = incomingAction;
    switch (action.type) {
        case actionTypes_1.REQUEST:
            return {
                loading: true
            };
        case actionTypes_1.REQUEST_RESPONSE:
            return {
                loading: false
            };
    }
    return state;
};
//# sourceMappingURL=LoaderReducer.js.map