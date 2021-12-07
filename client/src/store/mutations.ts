import { State } from "@/models/store";
import { MutationTree } from "vuex";

export const mutations: MutationTree<State> = {
  pageReady(state: State): void {
    state.pageStatus = "ready";
  },
  pageError(state: State): void {
    state.pageStatus = "error";
  }
};
