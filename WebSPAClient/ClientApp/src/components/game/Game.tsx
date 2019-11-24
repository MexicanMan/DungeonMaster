import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as GameReducer from '../../store/game/GameReducer';
import { ApplicationState } from '../../store';
import ModalWindow from '../helpers/ModalWindow';
import BaseGameWindow from './BaseGameWindow';
import { faEllipsisV } from '@fortawesome/free-solid-svg-icons/faEllipsisV';
import { faArrowUp } from '@fortawesome/free-solid-svg-icons/faArrowUp';
import { faArrowDown } from '@fortawesome/free-solid-svg-icons/faArrowDown';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons/faArrowLeft';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons/faArrowRight';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Field from './Field';
import Controller from './Controller';

import './Room.css';

type GameProps =
    GameReducer.GameState // ... state we've requested from the Redux store
    & typeof GameReducer.actionCreators;

class Game extends Component<GameProps> {
    constructor(props: GameProps) {
        super(props);

        this.alertClosed = this.alertClosed.bind(this)
    }

    componentDidMount() {
        this.props.requestEnterGame();
    }

    componentWillUnmount() {
        this.props.cleanGameStates();
    }

    alertClosed() {
        
    }

    render() {
        return (
            <BaseGameWindow>
                <div id="gameRow" className="row no-gutters">
                    <Field />
                    <Controller />
                </div>
                <div id="logRow" className="row no-gutters" hidden>
                    <div className="row no-gutters ml-2"><span className="col-12">Text log:</span></div>
                    <div id="log" className="row no-gutters border rounded border-dark overflow-auto col-12">
                        <div id="logText" className="ml-2">
                            2019-11-21 13:34 You came to the room #3 discovered by Nickname!<br/>
                            2019-11-21 13:35 You created a new room #10!<br />
                            2019-11-21 13:36 You hit monster and it hit you back!<br />
                            2019-11-21 13:37 You hit monster and it died!<br />
                            2019-11-21 13:39 You pickuped treasure!<br />
                        </div>
                    </div>
                </div>
            </BaseGameWindow>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.game,
    };
}

export default connect(mapStateToProps, GameReducer.actionCreators)(Game as any);