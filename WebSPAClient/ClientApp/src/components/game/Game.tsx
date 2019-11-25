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
import Log from './Log';

type GameProps = typeof GameReducer.actionCreators;

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
                <div id="logRow" className="no-gutters">
                    <Log />
                </div>
            </BaseGameWindow>
        );
    }
}

export default connect(null, GameReducer.actionCreators)(Game as any);