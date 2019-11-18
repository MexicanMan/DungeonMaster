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
import FetchData from './components/FetchData';
import Counter from './components/Counter';
import * as Path from './routes';

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <Switch>
                <Route exact path="/" component={App} />
                <Route path={Path.AUTH} component={Auth} />
                <Route path={Path.MAIN_MENU} component={Counter} />
            </Switch>
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'));

registerServiceWorker();
