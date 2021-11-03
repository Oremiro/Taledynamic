import { State, UserState } from "@/interfaces/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<UserState, State> = {
	id(state): number | null{
		return state.user.id;
	},
	email(state): string {
		return state.user.email;
	},
	accessToken(state): string {
		return state.accessTokenInMemory;
	},
	isLoggedIn(state): boolean {
		return state.accessTokenInMemory != '';
	}
}
