import React, { Component } from 'react';
import { connect } from 'react-redux';
import * as LogReducer from '../../store/game/LogReducer';
import { ApplicationState } from '../../store';

type LogProps =
    LogReducer.LogState // ... state we've requested from the Redux store
    & typeof LogReducer.actionCreators;

class Log extends Component<LogProps> {
    constructor(props: LogProps) {
        super(props);
    }

    render() {
        return (
            <div>
                <div className="row ml-2">Text log:</div>
                <div id="log" className="row no-gutters border rounded border-dark overflow-auto w-100">
                    <div id="logText" className="ml-2 w-100">
                        {this.props.log.map((action, i) => (
                            <div key={i}>{action}</div>
                        ))}
                    </div>
                </div>
            </div>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.log,
    };
}

export default connect(mapStateToProps, LogReducer.actionCreators)(Log as any);