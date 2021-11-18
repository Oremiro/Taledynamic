import { WorkspaceApi } from "@/helpers/api/workspace";
import { State, WorkspacesState, WorkspacesSortType } from "@/interfaces/store";
import { ActionTree } from "vuex";
import axios from "axios";

export const actions: ActionTree<WorkspacesState, State> = {
  async init({ commit, rootGetters }): Promise<void> {
    try {
      const { data } = await WorkspaceApi.getByUser(
        rootGetters["user/accessToken"]
      );
      commit("setWorkspaces", { workspaces: data.workspaces });
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 401) {
          // TODO: 401 handler
          throw new Error("Access token is incorrect");
        } else {
          throw new Error(error.response?.statusText);
        }
      }
    }
  },
  async create(
    { commit, dispatch, state, rootGetters },
    payload: { name: string }
  ): Promise<void> {
    try {
      const { data } = await WorkspaceApi.create(
        { name: payload.name },
        rootGetters["user/accessToken"]
      );
      commit("add", { workspace: data.workspace });
      await dispatch("sort", { sortType: state.sortType });
    } catch (error) {
      if (axios.isAxiosError(error)) {
        if (error.response?.status === 401) {
          // TODO: 401 handler
          throw new Error("Access token is incorrect");
        } else {
          throw new Error(error.response?.statusText);
        }
      }
    }
  },
  async delete(
    { commit, state, rootGetters },
    payload: { id: number }
  ): Promise<void> {
    const workspaceIndex = state.workspaces.findIndex(
      (item) => item.id === payload.id
    );
    if (~workspaceIndex) {
      try {
        await WorkspaceApi.delete(
          { id: payload.id },
          rootGetters["user/accessToken"]
        );
        commit("delete", { workspaceIndex: workspaceIndex });
      } catch (error) {
        if (axios.isAxiosError(error)) {
          if (error.response?.status === 401) {
            // TODO: 401 handler
            throw new Error("Access token is incorrect");
          } else {
            throw new Error(error.response?.statusText);
          }
        }
      }
    } else {
      throw new Error("Workspace id is incorrect");
    }
  },
  async update(
    { commit, dispatch, state, rootGetters },
    payload: { id: number; name: string }
  ): Promise<void> {
    const workspaceIndex = state.workspaces.findIndex(
      (item) => item.id === payload.id
    );
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
        if (payload.id === state.currentWorkspace?.id) {
          commit("setCurrent", { workspace: data.workspace });
        }
        await dispatch("sort", { sortType: state.sortType });
      } catch (error) {
        if (axios.isAxiosError(error)) {
          if (error.response?.status === 401) {
            // TODO: 401 handler
            throw new Error("Access token is incorrect");
          } else {
            throw new Error(error.response?.statusText);
          }
        }
      }
    } else {
      throw new Error("Workspace id is incorrect");
    }
  },
  async sort(
    { commit },
    payload: { sortType: WorkspacesSortType }
  ): Promise<void> {
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
