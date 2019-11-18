import { Action, Reducer, Dispatch } from 'redux';
import { AppThunkAction } from '../';
import { GATEWAY_ADDR } from '../../appconfig';
//import { } from '../actionTypes';
import { actionCreators as loaderActionCreators } from '../helpers/LoaderReducer';
import { push } from 'connected-react-router';
import * as Path from '../../routes';

export const actionCreators = {
    moveToGame: () => (dispatch: Dispatch) => { dispatch(push(Path.MAIN_MENU)); },
};