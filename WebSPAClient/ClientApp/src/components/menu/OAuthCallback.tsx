import React, { Component } from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import * as LoaderReducer from '../../store/helpers/LoaderReducer';
import Loader from '../helpers/Loader';
import { ApplicationState } from '../../store';
import * as queryString from 'query-string';

type OAuthProps =
    typeof LoaderReducer.actionCreators & 
    RouteComponentProps<any>;

class OAuthCallback extends Component<OAuthProps> {
    constructor(props: OAuthProps) {
        super(props);
    }

    componentDidMount() {
        this.props.request();

        const values = queryString.parse(this.props.location.search);
        this.props.requestOAuth(values.code as string);
    }

    componentWillUnmount() {
        this.props.response();
    }

    render() {
        return (
            <div>
                <Loader />
            </div>
        );
    }
}

function mapStateToProps(state: ApplicationState) {
    return {
        ...state.loader,
    };
}

export default connect(mapStateToProps, LoaderReducer.actionCreators)(OAuthCallback as any);