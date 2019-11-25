import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as LeaderboardReducer from '../../store/menu/LeaderboardReducer';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons/faArrowLeft';
import { faArrowRight } from '@fortawesome/free-solid-svg-icons/faArrowRight';
import BaseGameWindow from '../game/BaseGameWindow';

type LeaderboardProps =
    LeaderboardReducer.LeaderboardState // ... state we've requested from the Redux store
    & typeof LeaderboardReducer.actionCreators;

class Leaderboard extends Component<LeaderboardProps> {
    constructor(props: LeaderboardProps) {
        super(props);

        this.modalErrorClose = this.modalErrorClose.bind(this);
    }

    modalErrorClose() {

    }

    componentDidMount() {
        this.props.requestLeaderboardPage(this.props.currentPage);
    }

    render() {
        let prevDis = this.props.isPrev ? "" : "disabled";
        let nextDis = this.props.isNext ? "" : "disabled";

        return (
            <BaseGameWindow>
                <button type="button" id="backBtn" className="btn btn-primary m-2" onClick={() => { this.props.moveToMainMenu(); }}>
                    To menu
                </button>
                <table className="table table-striped">
                    <thead>
                        <tr className="d-flex">
                            <th className="col-1">№</th>
                            <th className="col-7">Player</th>
                            <th className="col-2">Treasures</th>
                            <th className="col-2">Death Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.players.map((player, index) => (
                            <tr id="player" key={index} className="d-flex">
                                <td className="col-1">{index + 1 + 10 * this.props.currentPage}</td>
                                <td className="col-7">{player.username}</td>
                                <td className="col-2">{player.treasureCount}</td>
                                <td className={"col-2 " + (player.isDead ? "text-danger" : "text-success")}>
                                    {player.isDead ? "Dead" : "Alive"}
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                <nav>
                    <ul className="pagination justify-content-center">
                        <li className={"page-item " + prevDis}>
                            <a className="page-link" tabIndex={-1}
                                onClick={() => this.props.requestLeaderboardPage(this.props.currentPage - 1)}>
                                <span><FontAwesomeIcon className="mr-1" icon={faArrowLeft} /></span> Prev
                            </a>
                        </li>
                        <li className={"page-item " + nextDis}>
                            <a className="page-link" tabIndex={-1}
                                onClick={() => this.props.requestLeaderboardPage(this.props.currentPage + 1)}>
                                Next <span><FontAwesomeIcon className="ml-1" icon={faArrowRight} /></span> 
                            </a>
                        </li>
                    </ul>
                </nav>
            </BaseGameWindow>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.leaderboard,
    };
}

export default connect(mapStateToProps, LeaderboardReducer.actionCreators)(Leaderboard as any);