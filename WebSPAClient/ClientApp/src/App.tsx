import React, { Component } from 'react';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import Auth from './components/menu/Auth'

class App extends Component     {
    render() {
        return (
            <div>
                <Auth />
            </div>
        );
    }
}

export default App;
/*export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
    </Layout>
);*/
