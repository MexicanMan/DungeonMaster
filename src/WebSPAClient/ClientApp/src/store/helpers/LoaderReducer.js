"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var actionTypes_1 = require("../actionTypes");
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    request: function () { return ({ type: actionTypes_1.REQUEST }); },
    response: function () { return ({ type: actionTypes_1.REQUEST_RESPONSE }); }
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