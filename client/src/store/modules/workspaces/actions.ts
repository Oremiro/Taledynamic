import { WorkspaceApi } from "@/helpers/api/workspace";
import { State, WorkspacesState, SortType } from "@/interfaces/store";
import { ActionTree } from "vuex";
import axios from "axios";


export const actions: ActionTree<WorkspacesState, State> = {
	async init({ commit, rootGetters }): Promise<void> {
		try {
			const { data } = await WorkspaceApi.getByUser(rootGetters['user/accessToken']);
			commit('setWorkspaces', { workspaces: data.workspaces });
		} catch (error) {
			if (axios.isAxiosError(error)) {
				if (error.response?.status === 401) {
					// TODO: 401 handler
					throw new Error('Access token is incorrect')
				} else {
					throw new Error(error.response?.statusText)
				}
			}
		}
	},
	async create({ commit, rootGetters }, payload: { name: string }): Promise<void> {
		try {
			const { data } = await WorkspaceApi.create({ name: payload.name }, rootGetters['user/accessToken'])
			commit('add', { workspace: data.workspace })
		} catch (error) {
			if (axios.isAxiosError(error)) {
				if (error.response?.status === 401) {
					// TODO: 401 handler
					throw new Error('Access token is incorrect')
				} else {
					throw new Error(error.response?.statusText)
				}
			}
		}
	},
	async delete({ commit, state, rootGetters }, payload: { id: number }): Promise<void> {
		const workspaceIndex = state.workspaces.findIndex(item => item.id === payload.id);
		if (~workspaceIndex) {
			try {
				await WorkspaceApi.delete({ id: payload.id }, rootGetters['user/accessToken'])
				commit('delete', { workspaceIndex: workspaceIndex })
			} catch (error) {
				if (axios.isAxiosError(error)) {
					if (error.response?.status === 401) {
						// TODO: 401 handler
						throw new Error('Access token is incorrect')
					} else {
						throw new Error(error.response?.statusText);
					}
				}
			}
		} else {
			throw new Error('Workspace id is incorrect')
		}
	},
	async update({ commit, state, rootGetters }, payload: { id: number, name: string }): Promise<void> {
		const workspaceIndex = state.workspaces.findIndex(item => item.id === payload.id);
		if (~workspaceIndex) {
			try {
				const { data } = await WorkspaceApi.update({ id: payload.id, name: payload.name }, rootGetters['user/accessToken'])
				commit('update', { oldWorkspaceIndex: workspaceIndex, newWorkspace: data.workspace });
			} catch (error) {
				if(axios.isAxiosError(error)) {
					if (error.response?.status === 401) {
						// TODO: 401 handler
						throw new Error('Access token is incorrect')
					} else {
						throw new Error(error.response?.statusText);
					}
				}
			}
		} else {
			throw new Error('Workspace id is incorrect')
		}
	},
	async sort({ commit }, payload: { sortType: SortType }): Promise<void> {
		switch (payload.sortType) {
			case SortType.NameAscending: {
				commit('sortByNameAscending');
				break;
			}
			case SortType.NameDescending: {
				commit('sortByNameDescending');
				break;
			}
			case SortType.DateAscending: {
				commit('sortByDateAscending');
				break;
			}
			case SortType.DateDescending: {
				commit('sortByDateDescending');
				break;
			}
		}
	}
}