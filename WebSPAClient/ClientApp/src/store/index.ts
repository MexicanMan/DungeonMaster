import * as WeatherForecasts from './WeatherForecasts';
import * as Counter from './Counter';
import * as Auth from './menu/AuthReducer';
import * as Leaderboard from './menu/LeaderboardReducer';
import * as Loader from './helpers/LoaderReducer';
import * as Game from './game/GameReducer';

// The top-level state object
export interface ApplicationState {
    counter: Counter.CounterState | undefined;
    weatherForecasts: WeatherForecasts.WeatherForecastsState | undefined;
    auth: Auth.AuthState | undefined;
    loader: Loader.LoaderState | undefined;
    game: Game.GameState | undefined;
    leaderboard: Leaderboard.LeaderboardState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    counter: Counter.reducer,
    weatherForecasts: WeatherForecasts.reducer,
    auth: Auth.authReducer,
    loader: Loader.loaderReducer,
    game: Game.gameReducer,
    leaderboard: Leaderboard.leaderboardReducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
