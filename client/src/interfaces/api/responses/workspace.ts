import BaseResponse from "@/interfaces/api/responses/base";

export interface Workspace {
	id: number,
	name: string,
	created: string,
	modified: string,
	userId: number
}

export interface CreateWorkspaceResponse extends BaseResponse {
	workspace: Workspace
}

export interface DeleteWorkspaceResponse extends BaseResponse {}

export interface UpdateWorkspaceResponse extends CreateWorkspaceResponse {}

export interface GetWorkspaceByIdResponse extends BaseResponse {
	workspace: Workspace
}

export interface GetWorkspacesByUserResponse extends BaseResponse {
	workspaces: Workspace[]
}