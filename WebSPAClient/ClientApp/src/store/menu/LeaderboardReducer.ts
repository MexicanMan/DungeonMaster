import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR, GAME_CONTROL_ADDR } from '../../appconfig';
import { LEADERBOARD_UPDATE_PAGE } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import ErrorResponse from '../helpers/ErrorResponse';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';

const PAGE_SIZE = 10;

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface LeaderboardState {
    currentPage: number;
    isPrev: boolean;
    isNext: boolean;
    players: PlayerLeaderboard[];
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface UpdateLeaderboardPageAction {
    type: typeof LEADERBOARD_UPDATE_PAGE;
    currentPage: number;
    isPrev: boolean;
    isNext: boolean;
    players: PlayerLeaderboard[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = UpdateLeaderboardPageAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export interface LeaderboardResponse {
    players: PlayerLeaderboard[];
}

export interface PlayerLeaderboard {
    username: string;
    treasureCount: number;
    isDead: boolean;
}

export const actionCreators = {
    requestLeaderboardPage: (page: number): AppThunkAction<any> => (dispatch) => {
        dispatch(loaderActionCreators.request());

        fetch(`${GATEWAY_ADDR}${GAME_CONTROL_ADDR}/leaderboard?page=${page}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json',
                'Authorization': `${sessionStorage.getItem("scheme")} ${sessionStorage.getItem("auth_token")}`
            }
        })
            .then(response => {
                if (response.ok)
                    return response.json() as Promise<LeaderboardResponse>;
                else
                    throw (response.json() as Promise<ErrorResponse>);
            })
            .then(data => {
                console.log(data);
                if (data.players.length > 0)
                    dispatch({
                        type: LEADERBOARD_UPDATE_PAGE, currentPage: page,
                        isPrev: page == 0 ? false : true,
                        isNext: data.players.length == PAGE_SIZE ? true : false,
                        players: data.players
                    })
                else
                    dispatch({
                        type: LEADERBOARD_UPDATE_PAGE, currentPage: page,
                        isPrev: page == 0 ? false : true, isNext: false,
                        players: []
                    })

                dispatch(loaderActionCreators.response());
            })
            
            .catch(error => {
                console.log(error);
            });
    },

    moveToMainMenu: () => (dispatch: Dispatch) => { dispatch(push(Path.MAIN_MENU)); },
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const initialState: LeaderboardState = {
    currentPage: 0,
    isPrev: false,
    isNext: true,
    players: []
};

export const leaderboardReducer: Reducer<LeaderboardState> = (state: LeaderboardState = initialState, incomingAction: Action): LeaderboardState => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case LEADERBOARD_UPDATE_PAGE:
            return {
                currentPage: action.currentPage,
                isPrev: action.isPrev,
                isNext: action.isNext,
                players: action.players
            };
    }

    return state;
};