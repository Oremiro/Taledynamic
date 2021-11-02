import { CreateWorkspaceResponse, UpdateWorkspaceResponse, DeleteWorkspaceResponse, GetWorkspaceByIdResponse, GetWorkspacesByUserResponse } from "@/interfaces/api/responses";
import { AxiosResponse } from "axios";
import { Api } from "@/helpers/api/base";


export class WorkspaceApi extends Api {
	static create(): Promise<AxiosResponse<CreateWorkspaceResponse>> {
		return this.axiosInstance.post<CreateWorkspaceResponse>(
			'/data/workspace/create',
		) 
	}

	static delete(): Promise<AxiosResponse<DeleteWorkspaceResponse>> {
		return this.axiosInstance.delete<DeleteWorkspaceResponse>(
			'/data/workspace/delete',
		) 
	}

	static update(): Promise<AxiosResponse<UpdateWorkspaceResponse>> {
		return this.axiosInstance.put<UpdateWorkspaceResponse>(
			'/data/workspace/update',
		) 
	}

	static get(): Promise<AxiosResponse<GetWorkspaceByIdResponse>> {
		return this.axiosInstance.get<GetWorkspaceByIdResponse>(
			'/data/workspace/get',
		) 
	}

	static getByUser(): Promise<AxiosResponse<GetWorkspacesByUserResponse>> {
		return this.axiosInstance.get<GetWorkspacesByUserResponse>(
			'/data/workspace/get-filtered-by-user',
		) 
	}
}