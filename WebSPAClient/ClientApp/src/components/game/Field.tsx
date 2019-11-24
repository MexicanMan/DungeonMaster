import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as FieldReducer from '../../store/game/FieldReducer';
import { ApplicationState } from '../../store';
import { faHeart } from '@fortawesome/free-solid-svg-icons/faHeart';
import { faStarOfLife } from '@fortawesome/free-solid-svg-icons/faStarOfLife';
import { faWindowMinimize } from '@fortawesome/free-regular-svg-icons/faWindowMinimize';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import * as RoomSymbolTypes from '../../store/helpers/RoomSymbolTypes';
import { MonsterTypes } from '../../store/helpers/MonsterTypes';

type FieldProps =
    FieldReducer.FieldState // ... state we've requested from the Redux store
    & typeof FieldReducer.actionCreators;

class Field extends Component<FieldProps> {
    constructor(props: FieldProps) {
        super(props);

        this.alertClosed = this.alertClosed.bind(this)
    }

    alertClosed() {

    }

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

    fromMonsterTypeToColor(type: MonsterTypes) : string {
        switch (type) {
            case MonsterTypes.Easy:
                return "text-success";
            case MonsterTypes.Medium:
                return "text-warning";
            case MonsterTypes.Hard:
                return "text-danger";
        }

        return "text-white";
    }

    render() {
        let monsterRowInvis: string = "invisible";
        let monsterTypeColor: string = "text-white";
        if (this.props.monsterType != undefined) {
            monsterRowInvis = "";
            monsterTypeColor = this.fromMonsterTypeToColor(this.props.monsterType);
        }

        return (
            <div id="fieldSide" className="col-6">
                <div id="field" className="text-white bg-dark m-2">
                    <div id="monsterRow" className="row ml-1 mr-1 pt-2">
                        <div className={"col-sm-12 " + monsterRowInvis}>
                            <span className="mr-1">{this.props.monsterHP}/{this.props.monsterMaxHP}</span> <strong className={monsterTypeColor}>M</strong>
                        </div>
                    </div>
                    <div id="room" className="row no-gutters">
                        <table className="room-symbol offset-3 col-6">
                            <tbody>
                                {this.props.room.map((row, index) => (
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
                            <span className="mr-1">{this.props.playerHP}/{this.props.playerMaxHP}</span> <FontAwesomeIcon color="red" icon={faHeart} />
                        </div>
                        <div className="col-sm-7">
                            <span className="float-right">{sessionStorage.getItem("username")}</span>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.field,
    };
}

export default connect(mapStateToProps, FieldReducer.actionCreators)(Field as any);