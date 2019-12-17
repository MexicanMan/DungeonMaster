import React, { Component } from "react";
import Modal from 'react-responsive-modal';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExclamationTriangle } from '@fortawesome/free-solid-svg-icons/faExclamationTriangle';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons/faInfoCircle';
import Leaderboard from "../menu/Leaderboard";

type ModalLeaderboardProps = {
    open: boolean,
    onClose: () => void,
}

class ModalLeaderboard extends Component<ModalLeaderboardProps> {
    constructor(props: ModalLeaderboardProps) {
        super(props);
    }

    render() {
        return (
            <Modal role="info" classNames={{ modal: "w-100 alert alert-light" }} open={this.props.open} onClose={this.props.onClose}>
                <div className="mt-5">
                    <Leaderboard />
                </div>
            </Modal>
        );
    }
}

export default ModalLeaderboard;