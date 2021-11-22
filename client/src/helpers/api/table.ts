import {
  TableCreateRequest,
  TableDeleteRequest,
  TableGetListRequest,
  TableGetRequest,
  TableUpdateRequest
} from "@/models/api/requests";
import {
  TableCreateResponse,
  TableDeleteResponse,
  TableGetListResponse,
  TableGetResponse,
  TableUpdateResponse
} from "@/models/api/responses";
import axios, { AxiosPromise } from "axios";

export class TableApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/data/table",
    withCredentials: true
  });

  static create(
    data: TableCreateRequest,
    accessToken: string
  ): AxiosPromise<TableCreateResponse> {
    return this.axiosInstance.post<TableCreateResponse>("/create", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static delete(
    params: TableDeleteRequest,
    accessToken: string
  ): AxiosPromise<TableDeleteResponse> {
    return this.axiosInstance.delete<TableDeleteResponse>("/delete", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static update(
    data: TableUpdateRequest,
    accessToken: string
  ): AxiosPromise<TableUpdateResponse> {
    return this.axiosInstance.put<TableUpdateResponse>("/update", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static get(
    params: TableGetRequest,
    accessToken: string
  ): AxiosPromise<TableGetResponse> {
    return this.axiosInstance.get<TableGetResponse>("/get", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
  static getList(
    params: TableGetListRequest,
    accessToken: string
  ): AxiosPromise<TableGetListResponse> {
    return this.axiosInstance.get<TableGetListResponse>(
      "/get-filtered-by-workspace",
      {
        params,
        headers: {
          Authorization: `Bearer ${accessToken}`
        }
      }
    );
  }
}
