import React, {Component, ChangeEvent, FormEvent } from 'react';
import { bindActionCreators, Dispatch } from 'redux';
import { connect } from 'react-redux';
//import { withRouter } from 'react-router-dom';
//import * as authActions from '../../actions/authActions.jsx';
//import * as paths from '../../routes/Paths.js';
import BaseWindow from './BaseWindow';
import * as AuthReducer from '../../store/menu/AuthReducer';
import { ApplicationState } from '../../store';
import { RouteComponentProps } from 'react-router';
import Loader from '../helpers/Loader';
//import FloatError from '../../Errors/floatError.jsx';

type AuthProps =
    AuthReducer.AuthState // ... state we've requested from the Redux store
    & typeof AuthReducer.actionCreators & // ... plus action creators we've requested
    RouteComponentProps<{}>;

type AuthState = {
    credentials: {
        [key: string]: string;
        username: string;
        password: string;
    }
}

class Auth extends Component<AuthProps, AuthState> {
    constructor(props: AuthProps) {
        super(props);

        this.state = {
            credentials: {
                username: '',
                password: ''
            }
        };

        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.registerClicked = this.registerClicked.bind(this);
        //this.alertClosed = this.alertClosed.bind(this)
    }

    registerClicked() {
        //this.props.history.push(paths.REGISTER);
    }

    /*alertClosed(event) {
        this.props.actions.authErrorClean();
    }*/

    handleChange(event: ChangeEvent<HTMLInputElement>) {
        const field = event.currentTarget.name;
        const credentials = this.state.credentials;
        credentials[field] = event.currentTarget.value;
        this.setState({ credentials: credentials });
    }

    handleSubmit(event: FormEvent) {
        event.preventDefault();
        this.props.requestAuth(this.state.credentials.username, this.state.credentials.password);
        //this.props.actions.loginUser(this.state.credentials, this.props.history);
    }

    render() {
        return (
            <BaseWindow header="Sign In">
                <hr />
                <form onSubmit={this.handleSubmit} className="needs-validation" noValidate>
                    <div className="form-group">
                        <label htmlFor="username">Username:</label>
                        <input type="username" className="form-control" placeholder="Enter username" id="username"
                            name="username" value={this.state.credentials.username} onChange={this.handleChange} required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="pwd">Password:</label>
                        <input type="password" className="form-control" placeholder="Enter password" id="pwd"
                            name="password" value={this.state.credentials.password} onChange={this.handleChange} required />
                    </div>
                    <button type="submit" className="btn btn-primary mb-3">Sign In</button>
                    <button type="button" className="btn btn-primary mb-3 float-right" onClick={this.registerClicked}>
                        Register
                    </button>
                </form>
                <Loader loading={false} />
            </BaseWindow>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.auth,
    };
}

export default connect(mapStateToProps, AuthReducer.actionCreators)(Auth as any);