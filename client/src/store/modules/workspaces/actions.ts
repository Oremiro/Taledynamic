import { WorkspaceApi } from "@/helpers/api/workspace";
import { State, WorkspacesState, WorkspacesSortType } from "@/models/store";
import { InitializationStatus } from "@/models";
import { ActionTree } from "vuex";
import axios from "axios";

export const actions: ActionTree<WorkspacesState, State> = {
  async init({ commit, rootGetters, dispatch }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    commit("setInitStatus", { initStatus: InitializationStatus.Pending });
    try {
      const { data } = await WorkspaceApi.getByUser(rootGetters["user/accessToken"]);
      commit("setWorkspaces", { workspaces: data.workspaces });
      commit("setInitStatus", { initStatus: InitializationStatus.Success });
    } catch (error) {
      commit("setInitStatus", { initStatus: InitializationStatus.Error });
      if (axios.isAxiosError(error)) {
        throw new Error(error.message);
      }
    }
  },
  async create({ commit, dispatch, state, rootGetters }, payload: { name: string }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    try {
      const { data } = await WorkspaceApi.create({ name: payload.name }, rootGetters["user/accessToken"]);
      commit("add", { workspace: data.workspace });
      await dispatch("sort", { sortType: state.sortType });
    } catch (error) {
      if (axios.isAxiosError(error)) {
        throw new Error(error.response?.statusText);
      }
    }
  },
  async delete({ commit, state, dispatch, rootGetters }, payload: { id: number }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    const workspaceIndex = state.workspaces.findIndex((item) => item.id === payload.id);
    if (~workspaceIndex) {
      try {
        await WorkspaceApi.delete({ id: payload.id }, rootGetters["user/accessToken"]);
        commit("delete", { workspaceIndex: workspaceIndex });
      } catch (error) {
        if (axios.isAxiosError(error)) {
          throw new Error(error.response?.statusText);
        }
      }
    } else {
      throw new Error("Workspace id is incorrect");
    }
  },
  async update({ commit, dispatch, state, rootGetters }, payload: { id: number; name: string }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    const workspaceIndex = state.workspaces.findIndex((item) => item.id === payload.id);
    if (~workspaceIndex) {
      try {
        const { data } = await WorkspaceApi.update(
          { id: payload.id, name: payload.name },
          rootGetters["user/accessToken"]
        );
        commit("update", {
          oldWorkspaceIndex: workspaceIndex,
          newWorkspace: data.workspace
        });
        if (payload.id === state.currentWorkspaceId) {
          commit("setCurrentId", { workspaceId: data.workspace.id });
        }
        await dispatch("sort", { sortType: state.sortType });
      } catch (error) {
        if (axios.isAxiosError(error)) {
          throw new Error(error.response?.statusText);
        }
      }
    } else {
      throw new Error("Workspace id is incorrect");
    }
  },
  async sort({ commit }, payload: { sortType: WorkspacesSortType }): Promise<void> {
    commit("setSortType", { sortType: payload.sortType });
    switch (payload.sortType) {
      case WorkspacesSortType.NameAscending: {
        commit("sortByNameAscending");
        break;
      }
      case WorkspacesSortType.NameDescending: {
        commit("sortByNameDescending");
        break;
      }
      case WorkspacesSortType.DateAscending: {
        commit("sortByDateAscending");
        break;
      }
      default: {
        commit("setSortType", { sortType: WorkspacesSortType.DateDescending });
        commit("sortByDateDescending");
        break;
      }
    }
  }
};
