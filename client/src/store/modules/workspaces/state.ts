import { WorkspacesSortType, WorkspacesState } from "@/interfaces/store";

export const state: WorkspacesState = {
	workspaces: [],
	currentWorkspace: null,
	sortType: WorkspacesSortType.DateDescending
};