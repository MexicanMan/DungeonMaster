import 'bootstrap/dist/css/bootstrap.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { Route, Switch } from 'react-router';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory } from 'history';
import configureStore from './store/configureStore';
import App from './App';
import Auth from './components/menu/Auth';
import registerServiceWorker from './registerServiceWorker';
import * as Path from './routes/routes';
import MainMenu from './components/menu/MainMenu';
import PrivateRoute from './routes/PrivateRoute';
import PublicRoute from './routes/PublicRoute';
import Game from './components/game/Game';
import Leaderboard from './components/menu/Leaderboard';
import Loader from './components/helpers/Loader';
import OAuthCallback from './components/menu/OAuthCallback';

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <Switch>
                <PublicRoute exact path="/" component={App} />
                <PublicRoute path={Path.AUTH} component={Auth} />
                <PublicRoute path="/oacallback" component={OAuthCallback} />
                <PrivateRoute path={Path.MAIN_MENU} component={MainMenu} />
                <PrivateRoute path={Path.GAME} component={Game} />
                <PrivateRoute path={Path.LEADERBOARD} component={Leaderboard} />
            </Switch>
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'));

registerServiceWorker();
