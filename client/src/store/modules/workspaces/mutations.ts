import { MutationTree } from "vuex";
import { Workspace, WorkspacesState } from "@/interfaces/store";
import { Workspace as ReceivedWorkspace } from "@/interfaces/api/responses";

function cloneWorkspace(workspace: Workspace | ReceivedWorkspace): Workspace { 
	return {
		id: workspace.id,
		name: workspace.name,
		created: new Date(workspace.created instanceof Date ? workspace.created.getTime() : workspace.created),
		modified: new Date(workspace.modified instanceof Date ? workspace.modified.getTime() : workspace.modified),
		userId: workspace.userId
	}
}

export const mutations: MutationTree<WorkspacesState> = {
	setWorkspaces(state: WorkspacesState, payload: { workspaces: Workspace[] | ReceivedWorkspace[] }): void {
		state.workspaces = []; 
		payload.workspaces.forEach((item: Workspace | ReceivedWorkspace) => {
			state.workspaces.push(cloneWorkspace(item));
		});
	},
	setCurrent(state: WorkspacesState, payload: { workspace: Workspace }): void {
		state.currentWorkspace = payload.workspace;
	},
	add(state: WorkspacesState, payload: { workspace: Workspace | ReceivedWorkspace }): void {
		state.workspaces.push(cloneWorkspace(payload.workspace));
	},
	delete(state: WorkspacesState, payload: { id: number }): void {
		const index = state.workspaces.findIndex(item => item.id === payload.id);
		if (~index) {
			state.workspaces.splice(index, 1);
		}
	},
	update(state: WorkspacesState, payload: { workspace: Workspace, name: string }): void {
		payload.workspace.name = payload.name;
	}
}


