import { MutationTree } from "vuex";
import { User, UserState } from "@/models/store";

export const mutations: MutationTree<UserState> = {
  setUser(state: UserState, payload: { user: User }): void {
    state.user = payload.user;
  },
  login(state: UserState, payload: { user: User; accessToken: string }): void {
    state.user = payload.user;
    state.accessTokenInMemory.value = payload.accessToken;
    state.accessTokenInMemory.expiresAt = new Date(Date.now() + 14 * 60000);
  },
  logout(state: UserState): void {
    state.user = { id: null, email: "" };
    state.accessTokenInMemory.value = "";
    state.accessTokenInMemory.expiresAt = null;
  }
};
