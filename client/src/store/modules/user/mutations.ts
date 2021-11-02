import { MutationTree } from "vuex";
import { User, UserState } from "@/interfaces/store";


export const mutations: MutationTree<UserState> = {
	setUser(state: UserState, payload: { user: User }): void {
		state.user = payload.user;
	},
	login(state: UserState, payload: { user: User, accessToken: string }): void {
		state.user = payload.user;
		state.accessTokenInMemory = payload.accessToken;
	},
	logout(state: UserState): void {
		state.user = { id: null, email: '' };
		state.accessTokenInMemory = '';
	}
}
