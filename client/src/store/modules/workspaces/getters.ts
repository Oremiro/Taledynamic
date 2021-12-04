import { InitializationStatus } from "@/models";
import { State, Workspace, WorkspacesState } from "@/models/store";
import { GetterTree } from "vuex";

export const getters: GetterTree<WorkspacesState, State> = {
  workspaces(state: WorkspacesState): Workspace[] {
    return state.workspaces;
  },
  workspacesLength(state: WorkspacesState): number {
    return state.workspaces.length;
  },
  currentWorkspaceId(state: WorkspacesState): number | null {
    return state.currentWorkspaceId;
  },
  initStatus(state: WorkspacesState): InitializationStatus {
    return state.initStatus;
  }
};
