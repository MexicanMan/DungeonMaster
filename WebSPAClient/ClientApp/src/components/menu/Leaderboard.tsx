import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../../store';
import * as LeaderboardReducer from '../../store/menu/LeaderboardReducer';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons/faArrowLeft';
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

    players: LeaderboardReducer.LeaderboardResponse = {
        players: [{ username: "AAA", treasureCount: 10, isDead: false },
        { username: "AAA", treasureCount: 10, isDead: true },
        { username: "AAA", treasureCount: 10, isDead: false },
        { username: "AAA", treasureCount: 10, isDead: false },
        { username: "AAA", treasureCount: 10, isDead: false },
        { username: "AAAAAAAAAAAA", treasureCount: 10, isDead: false },
        { username: "AAAAAAAAAAAA", treasureCount: 10, isDead: false },
        { username: "AAAAAAAAAAAA", treasureCount: 7, isDead: true },
        { username: "AAAAAAAAAAAA", treasureCount: 1, isDead: false },]
    }

    render() {
        return (
            <BaseGameWindow>
                <div className="">
                    <table className="table table-striped">
                        <thead>
                            <tr>
                                <th className="">№</th>
                                <th className="">Player</th>
                                <th className="">Treasures</th>
                                <th className="">Death Status</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.players.players.map((player, index) => (
                                <tr id="player" key={index}>
                                    <td className="">{index + 1}</td>
                                    <td className="">{player.username}</td>
                                    <td className="">{player.treasureCount}</td>
                                    <td className="">{player.isDead}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
                <button type="button" id="backBtn" className="btn btn-primary mb-2 ml-2" onClick={() => { }}>
                    <span><FontAwesomeIcon className="mr-1" icon={faArrowLeft} /></span> Back
                </button>
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