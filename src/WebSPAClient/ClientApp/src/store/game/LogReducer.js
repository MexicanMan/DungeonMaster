"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var actionTypes_1 = require("../actionTypes");
var LOG_SIZE = 20;
// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).
exports.actionCreators = {
    logUpdate: function (actionString) { return function (dispatch) {
        dispatch({
            type: actionTypes_1.LOG_UPDATE, actionLog: actionString
        });
    }; },
    cleanState: function () { return ({ type: actionTypes_1.LOG_CLEAN }); },
};
// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
exports.initialState = {
    log: [],
};
exports.logReducer = function (state, incomingAction) {
    if (state === void 0) { state = exports.initialState; }
    var knownAction = incomingAction;
    switch (knownAction.type) {
        case actionTypes_1.LOG_UPDATE: {
            var action = knownAction;
            var newLog = [];
            for (var i = 0; i < state.log.length; i++) {
                newLog.push(state.log[i]);
            }
            if (state.log.length > LOG_SIZE)
                newLog.shift();
            newLog.push(action.actionLog);
            return {
                log: newLog
            };
        }
        case actionTypes_1.LOG_CLEAN:
            return exports.initialState;
    }
    return state;
};
//# sourceMappingURL=LogReducer.js.map