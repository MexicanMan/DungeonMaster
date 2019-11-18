import React, { Component } from "react";
import Modal from 'react-responsive-modal';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faExclamationTriangle } from '@fortawesome/free-solid-svg-icons/faExclamationTriangle';
import { faInfoCircle } from '@fortawesome/free-solid-svg-icons/faInfoCircle';

type ModalProps = {
    open: boolean,
    isError: boolean,
    text: string,
    onClose: () => void,
}

class ModalWindow extends Component<ModalProps> {
    constructor(props: ModalProps) {
        super(props);
    }

    render() {
        if (this.props.isError)
            return (
                <Modal role="alert" classNames={{ modal: "w-100 alert alert-danger" }} open={this.props.open} onClose={this.props.onClose}>
                    <h2>Error!</h2>
                    <hr />
                    <span><FontAwesomeIcon icon={faExclamationTriangle} /></span>
                    <span className="pl-2">{this.props.text}</span>
                </Modal>
            );
        else
            return (
                <Modal role="info" classNames={{ modal: "w-100 alert alert-light" }} open={this.props.open} onClose={this.props.onClose}>
                    <h2>Information</h2>
                    <hr />
                    <span><FontAwesomeIcon icon={faInfoCircle} /></span>
                    <span className="pl-2">{this.props.text}</span>
                    <button type="button" className="btn btn-success float-right mt-3" onClick={this.props.onClose}>Ok!</button>
                </Modal>
            );
    }
}

export default ModalWindow;