import { State, UserState } from "@/models/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<UserState, State> = {
  id(state): number | null {
    return state.user.id;
  },
  email(state): string {
    return state.user.email;
  },
  accessToken(state): string {
    return state.accessTokenInMemory.value;
  },
  isLoggedIn(state): boolean {
    return state.accessTokenInMemory.value != "";
  },
  isAccessTokenExpired(state): boolean {
    if (state.accessTokenInMemory.expiresAt === null) return true;
    return state.accessTokenInMemory.expiresAt.getTime() < Date.now();
  }
};
