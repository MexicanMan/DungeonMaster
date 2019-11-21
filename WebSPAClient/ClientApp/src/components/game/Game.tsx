import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as GameReducer from '../../store/game/GameReducer';
import { ApplicationState } from '../../store';
import { RouteComponentProps } from 'react-router';
import ModalWindow from '../helpers/ModalWindow';
import BaseGameWindow from './BaseGameWindow';
import { faHeart } from '@fortawesome/free-solid-svg-icons/faHeart';
import { faStarOfLife } from '@fortawesome/free-solid-svg-icons/faStarOfLife';
import { faWindowMinimize } from '@fortawesome/free-regular-svg-icons/faWindowMinimize';
import { faEllipsisV } from '@fortawesome/free-solid-svg-icons/faEllipsisV';
import { faArrowUp } from '@fortawesome/free-solid-svg-icons/faArrowUp';
import { faArrowDown } from '@fortawesome/free-solid-svg-icons/faArrowDown';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons/faArrowLeft';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons/faArrowRight';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as RoomSymbolTypes from './RoomSymbolTypes';

import './Room.css';

type GameProps =
    GameReducer.GameState // ... state we've requested from the Redux store
    & typeof GameReducer.actionCreators;

class Game extends Component<GameProps> {
    constructor(props: GameProps) {
        super(props);

        this.alertClosed = this.alertClosed.bind(this)
    }

    alertClosed() {
        
    }

    roomMap: number[][] = [[ 1, 1, 13, 1,  1],
                           [ 1, 4,  0, 0,  1],
                           [12, 0,  3, 0, 11],
                           [ 1, 0,  2, 0,  1],
                           [ 1, 1, 14, 1,  1]];

    fromTypeToSymbol(type: number): JSX.Element {
        switch (type) {
            case RoomSymbolTypes.EMPTY:
                return (<span> </span>);
            case RoomSymbolTypes.WALL:
                return (<FontAwesomeIcon icon={faStarOfLife} />);
            case RoomSymbolTypes.PLAYER:
                return (<span>Y</span>);
            case RoomSymbolTypes.MONSTER:
                return (<span>M</span>);
            case RoomSymbolTypes.TREASURE:
                return (<span>T</span>);
            case RoomSymbolTypes.EAST_CLOSED:
                return (<span>|</span>);
            case RoomSymbolTypes.EAST_OPENED:
                return (<FontAwesomeIcon icon={faWindowMinimize} />);
            case RoomSymbolTypes.WEST_CLOSED:
                return (<span>|</span>);
            case RoomSymbolTypes.WEST_OPENED:
                return (<FontAwesomeIcon icon={faWindowMinimize} />);
            case RoomSymbolTypes.NORTH_CLOSED:
                return (<FontAwesomeIcon icon={faWindowMinimize} />);
            case RoomSymbolTypes.NORTH_OPENED:
                return (<span>|</span>);
            case RoomSymbolTypes.SOUTH_CLOSED:
                return (<FontAwesomeIcon icon={faWindowMinimize} />);
            case RoomSymbolTypes.SOUTH_OPENED:
                return (<span>|</span>);
        }

        return (<span> </span>);
    }

    render() {
        return (
            <BaseGameWindow>
                <div id="gameRow" className="row no-gutters">
                    <div id="fieldSide" className="col-6">
                        <div id="field" className="text-white bg-dark m-2">
                            <div id="monsterRow" className="row ml-1 mr-1 pt-2">
                                <div className="col-sm-12">
                                    <span className="mr-1">?/?</span> <strong className="text-white">M</strong>
                                </div>
                            </div>
                            <div id="room" className="row no-gutters">
                                <table className="room-symbol offset-3 col-6">
                                    <tbody>
                                        {this.roomMap.map((row, index) => (
                                            <tr id="roomRow" className="row pb-2" key={index}>
                                                {row.map((cell, cellIndex) => (
                                                    <td className="fix-col" key={cellIndex}>{this.fromTypeToSymbol(cell)}</td>
                                                ))}                                                    
                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                                </div>
                            <div id="playerRow" className="row ml-1 mr-1 pb-2">
                                <div className="col-sm-5">
                                    <span className="mr-1">3/3</span> <FontAwesomeIcon color="red" icon={faHeart} />
                                </div>
                                <div className="col-sm-7">
                                    <span className="float-right">Nick</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="controllerSide" className="col-6 pr-1">
                        <div id="menuRow" className="row m-2 p-1 no-gutters">
                            <div className="col-6">
                                <button type="button" className="btn btn-primary" onClick={() => { }}>
                                    <FontAwesomeIcon icon={faEllipsisV} className="mr-1" /> Menu
                                </button>
                            </div>
                            <div className="col-6">
                                <button type="button" className="btn btn-primary float-right" onClick={() => { }}>
                                    Leaderboard
                                </button>
                            </div>
                        </div>
                        <div id="treasureRow" className="row m-2 p-1 no-gutters justify-content-center">
                            <button type="button" className="btn btn-warning text-white col-md-5 col-sm-7" onClick={() => { }}>
                                Pickup Treasure
                            </button>
                        </div>
                        <div id="atkMonsterRow" className="row m-2 p-1 no-gutters justify-content-center">
                            <button type="button" className="btn btn-danger text-white col-md-5 col-sm-7" onClick={() => { }}>
                                Attack Monster
                            </button>
                        </div>
                        <div id="upRow" className="row m-2 pt-1 no-gutters justify-content-center">
                            <button type="button" className="btn btn-success text-white" onClick={() => { }}>
                                <FontAwesomeIcon icon={faArrowUp} />
                            </button>
                        </div>
                        <div id="ldrRow" className="row mt-2 ml-2 mr-2 no-gutters justify-content-center">
                            <button type="button" className="btn btn-success text-white ml-1 mr-1" onClick={() => { }}>
                                <FontAwesomeIcon icon={faArrowLeft} />
                            </button>
                            <button type="button" className="btn btn-success text-white ml-1 mr-1" onClick={() => { }}>
                                <FontAwesomeIcon icon={faArrowDown} />
                            </button>
                            <button type="button" className="btn btn-success text-white ml-1 mr-1" onClick={() => { }}>
                                <FontAwesomeIcon icon={faArrowRight} />
                            </button>
                        </div>
                    </div>
                </div>
                <div id="logRow" className="row no-gutters">
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