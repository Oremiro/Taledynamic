import { WorkspaceApi } from "@/helpers/api/workspace";
import { State, WorkspacesState } from "@/interfaces/store";
import { ActionTree } from "vuex";
import axios from "axios";

export const actions: ActionTree<WorkspacesState, State> = {
	async init({ commit, rootGetters }): Promise<void> {
		try {
			const { data } = await WorkspaceApi.getByUser(rootGetters['user/accessToken']);
			console.log(data.workspaces);
			commit('setWorkspaces', data.workspaces);
		} catch (error) {
			if (axios.isAxiosError(error)) {
				if (error.response?.status === 401) {
					// TODO: 401 handler
					throw new Error('Access token is incorrect')
				}
			}
		}
	},
	async create({ commit, rootGetters }, payload: { name: string }): Promise<void> {
		try {
			const { data } = await WorkspaceApi.create({ name: payload.name }, rootGetters['user/accessToken'])
			// TODO: commit('addWorkspace', data.workspace)
		} catch (error) {
			if (axios.isAxiosError(error)) {
				if (error.response?.status === 401) {
					// TODO: 401 handler
					throw new Error('Access token is incorrect')
				}
			}
		}
	},
	async delete({ commit, rootGetters }, payload: { id: number }): Promise<void> {
		try {
			await WorkspaceApi.delete({ id: payload.id }, rootGetters['user/accessToken'])
			commit('deleteWorkspace', payload.id)
		} catch (error) {
			if (axios.isAxiosError(error)) {
				if (error.response?.status === 401) {
					// TODO: 401 handler
					throw new Error('Access token is incorrect')
				}
			}
		}
	}
}