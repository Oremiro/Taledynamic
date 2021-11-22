import { InitializationStatus } from "@/models";
import { WorkspacesSortType, WorkspacesState } from "@/models/store";

export const state: WorkspacesState = {
  workspaces: [],
  currentWorkspace: null,
  sortType: WorkspacesSortType.DateDescending,
  initStatus: InitializationStatus.Pending
};
