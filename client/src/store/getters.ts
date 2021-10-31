import { State } from "@/interfaces/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<State, State> = {
	isLoggedIn(state): boolean {
		return state.accessTokenInMemory != '';
	}
}
