import { MutationTree } from "vuex";
import {
  Workspace,
  WorkspacesInitStatus,
  WorkspacesSortType,
  WorkspacesState
} from "@/interfaces/store";
import { Workspace as ReceivedWorkspace } from "@/interfaces/api/responses";

function cloneWorkspace(workspace: Workspace | ReceivedWorkspace): Workspace {
  return {
    id: workspace.id,
    name: workspace.name,
    created: new Date(
      workspace.created instanceof Date
        ? workspace.created.getTime()
        : workspace.created
    ),
    modified: new Date(
      workspace.modified instanceof Date
        ? workspace.modified.getTime()
        : workspace.modified
    ),
    userId: workspace.userId
  };
}

export const mutations: MutationTree<WorkspacesState> = {
  setWorkspaces(
    state: WorkspacesState,
    payload: { workspaces: Workspace[] | ReceivedWorkspace[] }
  ): void {
    state.workspaces = [];
    payload.workspaces.forEach((item: Workspace | ReceivedWorkspace) => {
      state.workspaces.push(cloneWorkspace(item));
    });
  },
  setCurrent(state: WorkspacesState, payload: { workspace: Workspace }): void {
    state.currentWorkspace = payload.workspace;
  },
  setSortType(
    state: WorkspacesState,
    payload: { sortType: WorkspacesSortType }
  ) {
    state.sortType = payload.sortType;
  },
  setInitStatus(state: WorkspacesState, payload: { initStatus: WorkspacesInitStatus }) {
    state.initStatus = payload.initStatus;
  },
  add(
    state: WorkspacesState,
    payload: { workspace: Workspace | ReceivedWorkspace }
  ): void {
    state.workspaces.push(cloneWorkspace(payload.workspace));
  },
  delete(state: WorkspacesState, payload: { workspaceIndex: number }): void {
    state.workspaces.splice(payload.workspaceIndex, 1);
  },
  update(
    state: WorkspacesState,
    payload: {
      oldWorkspaceIndex: number;
      newWorkspace: Workspace | ReceivedWorkspace;
    }
  ): void {
    state.workspaces[payload.oldWorkspaceIndex] = cloneWorkspace(
      payload.newWorkspace
    );
  },
  sortByDateAscending(state: WorkspacesState) {
    // От старых к новым
    state.workspaces.sort(
      (itemFirst, itemSecond) =>
        itemFirst.modified.getTime() - itemSecond.modified.getTime()
    );
  },
  sortByDateDescending(state: WorkspacesState) {
    // От новых к старым
    state.workspaces.sort(
      (itemFirst, itemSecond) =>
        itemSecond.modified.getTime() - itemFirst.modified.getTime()
    );
  },
  sortByNameAscending(state: WorkspacesState) {
    // По возрастанию (A -> Z)
    state.workspaces.sort((itemFirst, itemSecond) =>
      itemFirst.name.localeCompare(itemSecond.name)
    );
  },
  sortByNameDescending(state: WorkspacesState) {
    // По убыванию (Z -> A)
    state.workspaces.sort((itemFirst, itemSecond) =>
      itemSecond.name.localeCompare(itemFirst.name)
    );
  }
};
