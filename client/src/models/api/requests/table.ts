export interface TableCreateRequest {
  name: string;
  workspaceId: number;
}

export interface TableDeleteRequest {
  id: number;
}

export interface TableUpdateRequest {
  id: number;
  name: string;
}

export interface TableGetRequest {
  id: number;
  workspaceId: number;
}

export interface TableGetListRequest {
  workspaceId: number;
}
