import React, { PureComponent } from 'react';
import Loader from '../helpers/Loader';

// Base window background for all menu items
class BaseGameWindow extends PureComponent {
    render() {
        return (
            <div>
                <div className="row mt-2 no-gutters ml-1 mr-1">
                    <div className="col-sm-12 border rounded border-dark bg-light">
                        {this.props.children}
                    </div>
                </div>
                <Loader />
            </div>

        );
    }
}

export default BaseGameWindow;