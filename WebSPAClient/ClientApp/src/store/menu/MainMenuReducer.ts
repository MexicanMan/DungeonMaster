import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR } from '../../appconfig';
//import { } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import { push } from 'connected-react-router';
import * as Path from '../../routes/routes';

export const actionCreators = {
    moveToGame: () => (dispatch: Dispatch) => {
        dispatch(push(Path.GAME));
    },

    moveToLeaderboard: () => (dispatch: Dispatch) => {
        dispatch(push(Path.LEADERBOARD));
    },

    exit: () => (dispatch: Dispatch) => {
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("auth_token");
        sessionStorage.removeItem("refresh_token");
        sessionStorage.removeItem("expires_in");
        sessionStorage.removeItem("scheme");
        sessionStorage.removeItem("username");

        dispatch(push("/"));
    },
};