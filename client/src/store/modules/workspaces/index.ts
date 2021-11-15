import { Module } from "vuex";
import { state } from "@/store/modules/workspaces/state";
import { getters } from "@/store/modules/workspaces/getters";
import { mutations } from "@/store/modules/workspaces/mutations";
import { actions } from "@/store/modules/workspaces/actions";
import { State, WorkspacesState } from "@/interfaces/store";

export default {
  namespaced: true,
  state,
  getters,
  mutations,
  actions,
} as Module<WorkspacesState, State>;
