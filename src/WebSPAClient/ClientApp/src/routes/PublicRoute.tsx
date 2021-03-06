﻿import React from 'react';
import { Route, RouteProps, Redirect } from 'react-router';
import * as Paths from './routes';

// Route for redirecting not authorized users
class PublicRoute extends Route<RouteProps> {
    render() {
        if (sessionStorage.getItem("auth_token") == undefined || sessionStorage.getItem("auth_token") == '')
            return <Route {...this.props} />
        else {
            const renderComponent = () => (<Redirect to={{ pathname: Paths.MAIN_MENU }} />);
            return <Route {...this.props} component={renderComponent} render={undefined} />
        }
    }
}

export default PublicRoute;