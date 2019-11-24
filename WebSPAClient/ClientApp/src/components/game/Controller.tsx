import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as ControllerReducer from '../../store/game/ControllerReducer';
import { ApplicationState } from '../../store';
import ModalWindow from '../helpers/ModalWindow';
import { faEllipsisV } from '@fortawesome/free-solid-svg-icons/faEllipsisV';
import { faArrowUp } from '@fortawesome/free-solid-svg-icons/faArrowUp';
import { faArrowDown } from '@fortawesome/free-solid-svg-icons/faArrowDown';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons/faArrowLeft';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons/faArrowRight';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Directions } from '../../store/helpers/Directions';

type ControllerProps =
    ControllerReducer.ControllerState // ... state we've requested from the Redux store
    & typeof ControllerReducer.actionCreators;

class Controller extends Component<ControllerProps> {
    constructor(props: ControllerProps) {
        super(props);

        this.alertClosed = this.alertClosed.bind(this)
    }

    alertClosed() {

    }

    render() {
        return (
            <div id="controllerSide" className="col-6 pr-1">
                <div id="menuRow" className="row m-2 p-1 no-gutters">
                    <div className="col-6">
                        <button type="button" className="btn btn-primary" onClick={() => { this.props.moveToMenu(); }}>
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
                    <button type="button" className="btn btn-warning text-white col-md-5 col-sm-7"
                        disabled={!this.props.isTreasureActive} onClick={() => { this.props.treasurePkp(); }}>
                        Pickup Treasure
                    </button>
                </div>
                <div id="atkMonsterRow" className="row m-2 p-1 no-gutters justify-content-center">
                    <button type="button" className="btn btn-danger text-white col-md-5 col-sm-7"
                        disabled={!this.props.isMonsterActive} onClick={() => { this.props.monsterAtk(); }}>
                        Attack Monster
                    </button>
                </div>
                <div id="upRow" className="row m-2 pt-1 no-gutters justify-content-center">
                    <button type="button" className="btn btn-success text-white" disabled={!this.props.isUpActive}
                        onClick={() => { this.props.moveToRoom(Directions.North); }}>
                        <FontAwesomeIcon icon={faArrowUp} />
                    </button>
                </div>
                <div id="ldrRow" className="row mt-2 ml-2 mr-2 no-gutters justify-content-center">
                    <button type="button" className="btn btn-success text-white ml-1 mr-1" disabled={!this.props.isLeftActive}
                        onClick={() => { this.props.moveToRoom(Directions.West); }}>
                        <FontAwesomeIcon icon={faArrowLeft} />
                    </button>
                    <button type="button" className="btn btn-success text-white ml-1 mr-1" disabled={!this.props.isDownActive}
                        onClick={() => { this.props.moveToRoom(Directions.South); }}>
                        <FontAwesomeIcon icon={faArrowDown} />
                    </button>
                    <button type="button" className="btn btn-success text-white ml-1 mr-1" disabled={!this.props.isRightActive}
                        onClick={() => { this.props.moveToRoom(Directions.East); }}>
                        <FontAwesomeIcon icon={faArrowRight} />
                    </button>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.controller,
    };
}

export default connect(mapStateToProps, ControllerReducer.actionCreators)(Controller as any);