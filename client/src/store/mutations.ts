import { MutationTree } from "vuex";
import { State, User } from "@/interfaces/store";


export const mutations: MutationTree<State> = {
	setUser(state: State, payload: { user: User }): void {
		state.user = payload.user;
	},
	login(state: State, payload: { user: User, accessToken: string }): void {
		state.user = payload.user;
		state.accessTokenInMemory = payload.accessToken;
	},
	logout(state: State): void {
		state.user = { id: null, email: '' };
		state.accessTokenInMemory = '';
	},
	pageReady(state: State): void {
		state.pageStatus = 'ready';
	},
	pageError(state: State): void {
		state.pageStatus = 'error';
	}
}
