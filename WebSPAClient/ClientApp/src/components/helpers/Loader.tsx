import React, { Component } from 'react';

import './Loader.css';

type LoaderProps = {
    loading: boolean;
}

class Loader extends Component<LoaderProps> {
    constructor(props: LoaderProps) {
        super(props);
    }

    render() {
        if (!this.props.loading)
            return null;

        return (
            <div className="loader-overlay">
                <div className="sk-fading-circle">
                    <div className="sk-circle1 sk-circle"></div>
                    <div className="sk-circle2 sk-circle"></div>
                    <div className="sk-circle3 sk-circle"></div>
                    <div className="sk-circle4 sk-circle"></div>
                    <div className="sk-circle5 sk-circle"></div>
                    <div className="sk-circle6 sk-circle"></div>
                    <div className="sk-circle7 sk-circle"></div>
                    <div className="sk-circle8 sk-circle"></div>
                    <div className="sk-circle9 sk-circle"></div>
                    <div className="sk-circle10 sk-circle"></div>
                    <div className="sk-circle11 sk-circle"></div>
                    <div className="sk-circle12 sk-circle"></div>
                </div>
            </div>
        );
    }
}

export default Loader;