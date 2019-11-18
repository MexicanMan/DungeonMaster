import React, { Component, ChangeEvent, FormEvent } from 'react';
import { bindActionCreators, Dispatch } from 'redux';
import { connect } from 'react-redux';
//import { withRouter } from 'react-router-dom';
//import * as authActions from '../../actions/authActions.jsx';
//import * as paths from '../../routes/Paths.js';
import BaseWindow from './BaseWindow';
import * as MainMenuReducer from '../../store/menu/MainMenuReducer';
import { ApplicationState } from '../../store';
import { RouteComponentProps, withRouter } from 'react-router';
import ModalWindow from '../helpers/ModalWindow';

type MainMenuProps = typeof MainMenuReducer.actionCreators;

class MainMenu extends Component<MainMenuProps> {
    constructor(props: MainMenuProps) {
        super(props);

        this.enterGame = this.enterGame.bind(this);
        this.openLeaderboard = this.openLeaderboard.bind(this);
        this.exit = this.exit.bind(this);
    }

    enterGame() {
        this.props.moveToGame();
    }

    openLeaderboard() {

    }

    exit() {

    }

    render() {
        return (
            <BaseWindow header="Main Menu">
                <hr />
                <div className="row mb-3 ml-1">
                    <span>Player: <strong>{sessionStorage.getItem('username')}</strong></span>
                </div>
                <div className="row mb-3 justify-content-center">
                    <button type="button" className="btn btn-success w-50" onClick={() => this.enterGame()}>
                        Enter game
                    </button>
                </div>
                <div className="row mb-3 justify-content-center">
                    <button type="button" className="btn btn-primary w-50" onClick={() => this.openLeaderboard()}>
                        Leaderboard
                    </button>
                </div>
                <div className="row mb-3 justify-content-center">
                    <button type="button" className="btn btn-danger w-50" onClick={() => this.exit()}>
                        Exit
                    </button>
                </div>
            </BaseWindow>
        );
    }
}

export default connect(null, MainMenuReducer.actionCreators)(MainMenu as any);