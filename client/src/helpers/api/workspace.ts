import {
  CreateWorkspaceRequest,
  DeleteWorkspaceRequest,
  GetWorkspaceByIdRequest,
  UpdateWorkspaceRequest
} from "@/models/api/requests";
import {
  CreateWorkspaceResponse,
  UpdateWorkspaceResponse,
  DeleteWorkspaceResponse,
  GetWorkspaceByIdResponse,
  GetWorkspacesByUserResponse
} from "@/models/api/responses";
import axios, { AxiosPromise } from "axios";

export class WorkspaceApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/data/workspace",
    withCredentials: true
  });

  static create(data: CreateWorkspaceRequest, accessToken: string): AxiosPromise<CreateWorkspaceResponse> {
    return this.axiosInstance.post<CreateWorkspaceResponse>("/create", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static delete(params: DeleteWorkspaceRequest, accessToken: string): AxiosPromise<DeleteWorkspaceResponse> {
    return this.axiosInstance.delete<DeleteWorkspaceResponse>("/delete", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static update(data: UpdateWorkspaceRequest, accessToken: string): AxiosPromise<UpdateWorkspaceResponse> {
    return this.axiosInstance.put<UpdateWorkspaceResponse>("/update", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static get(params: GetWorkspaceByIdRequest, accessToken: string): AxiosPromise<GetWorkspaceByIdResponse> {
    return this.axiosInstance.get<GetWorkspaceByIdResponse>("/get", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static getByUser(accessToken: string): AxiosPromise<GetWorkspacesByUserResponse> {
    return this.axiosInstance.get<GetWorkspacesByUserResponse>("/get-filtered-by-user", {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
}
