import React, { PureComponent } from 'react';
import Loader from '../helpers/Loader';

type BaseWindowProps = { header: string };

// Base window background for all menu items
class BaseWindow extends PureComponent<BaseWindowProps> {
    constructor(props: BaseWindowProps) {
        super(props);
    }

    render() {
        return (
            <div>
                <div className="row mt-5">
                    <div className="col-sm-4"></div>
                    <div className="col-sm-4 border rounded border-dark bg-light">
                        <h3 className="mt-2">Dungeon Master | {this.props.header}</h3>
                        {this.props.children}
                    </div>
                    <div className="col-sm-4"></div>
                </div>
                <Loader />
            </div>
        );
    }
}

export default BaseWindow;