export interface CreateWorkspaceRequest {
  name: string;
}

export interface DeleteWorkspaceRequest {
  id: number;
}

export interface UpdateWorkspaceRequest {
  id: number;
  name: string;
}

export interface GetWorkspaceByIdRequest {
  id: number;
}
