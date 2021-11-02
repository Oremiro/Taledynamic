import { State, Workspace, WorkspacesState } from "@/interfaces/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<WorkspacesState, State> = {
	workspaces(state): Workspace[] {
		return state.workspaces
	}
}
