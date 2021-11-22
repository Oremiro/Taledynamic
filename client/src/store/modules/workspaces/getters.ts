import { InitializationStatus } from "@/models";
import { State, Workspace, WorkspacesState } from "@/models/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<WorkspacesState, State> = {
  workspaces(state: WorkspacesState): Workspace[] {
    return state.workspaces;
  },
  currentWorkspace(state: WorkspacesState): Workspace | null {
    return state.currentWorkspace;
  },
  initStatus(state: WorkspacesState): InitializationStatus {
    return state.initStatus;
  }
};
