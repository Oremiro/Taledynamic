import { State, Workspace, WorkspacesInitStatus, WorkspacesState } from "@/interfaces/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<WorkspacesState, State> = {
  workspaces(state: WorkspacesState): Workspace[] {
    return state.workspaces;
  },
  currentWorkspace(state: WorkspacesState): Workspace | null {
    return state.currentWorkspace;
  },
  initStatus(state: WorkspacesState): WorkspacesInitStatus {
    return state.initStatus;
  }
};
