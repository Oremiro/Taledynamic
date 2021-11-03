import { MutationTree } from "vuex";
import { Workspace, WorkspacesState } from "@/interfaces/store";


export const mutations: MutationTree<WorkspacesState> = {
	setWorkspaces(state: WorkspacesState, payload: { workspaces: Workspace[] }): void {
		// TODO: deep copy
		state.workspaces = JSON.parse(JSON.stringify(payload.workspaces));
	},
	addWorkspace(state: WorkspacesState, payload: { workspace: Workspace }): void {
		state.workspaces.push(payload.workspace);
	},
	deleteWorkspace(state: WorkspacesState, payload: { id: number }): void {
		const index = state.workspaces.findIndex(item => item.id === payload.id);
		if (~index) {
			state.workspaces.splice(index, 1);
		}
	}
}


