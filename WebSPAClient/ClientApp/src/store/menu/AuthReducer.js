"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var appconfig_1 = require("../../appconfig");
var actionTypes_1 = require("../actionTypes");
exports.actionCreators = {
    requestAuth: function (username, pwd) { return function (dispatch) {
        dispatch({ type: 'AUTH_REQ' });
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
            .then(function (response) { return response.json(); })
            .then(function (data) {
            sessionStorage.setItem('auth_token', data.auth_token);
            sessionStorage.setItem('id', data.id);
            dispatch({ type: actionTypes_1.AUTH_SUCCESS });
        });
    }; }
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
var initialState = {
    error: ''
};
exports.authReducer = function (state, incomingAction) {
    if (state === void 0) { state = initialState; }
    var action = incomingAction;
    switch (action.type) {
        case 'AUTH_SUCCESS':
            return {
                error: ''
            };
    }
    return state;
};
//# sourceMappingURL=AuthReducer.js.map