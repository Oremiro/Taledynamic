import { WorkspacesSortType, WorkspacesState, WorkspacesInitStatus } from "@/interfaces/store";

export const state: WorkspacesState = {
  workspaces: [],
  currentWorkspace: null,
  sortType: WorkspacesSortType.DateDescending,
  initStatus: WorkspacesInitStatus.Pending
};
