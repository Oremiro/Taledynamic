import { Module } from "vuex";
import { state } from "@/store/modules/table/state";
import { getters } from "@/store/modules/table/getters";
import { mutations } from "@/store/modules/table/mutations";
import { actions } from "@/store/modules/table/actions";
import { State, TableState } from "@/models/store";

export default {
  namespaced: true,
  state,
  getters,
  mutations,
  actions
} as Module<TableState, State>;
