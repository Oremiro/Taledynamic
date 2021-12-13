import axios from "axios";
import {
  TableDataCreateRequest,
  TableDataUpdateRequest,
  TableDataDeleteRequest,
  TableDataGetRequest
} from "@/models/api/requests";
import {
  TableDataGetResponse,
  TableDataCreateResponse,
  TableDataUpdateResponse,
  TableDataDeleteResponse
} from "@/models/api/responses";

export class TableDataApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/data/table",
    withCredentials: true
  });

  static async get(data: TableDataGetRequest, accessToken: string) {
    return this.axiosInstance.get<TableDataGetResponse>(`/${data.id}/data/get`, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static async create(data: TableDataCreateRequest, accessToken: string) {
    return this.axiosInstance.post<TableDataCreateResponse>(
      `/${data.id}/data/create`,
      { jsonContent: data.jsonContent },
      {
        headers: {
          Authorization: `Bearer ${accessToken}`
        }
      }
    );
  }

  static async update(data: TableDataUpdateRequest, accessToken: string) {
    return this.axiosInstance.put<TableDataUpdateResponse>("/data/update", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static async delete(data: TableDataDeleteRequest, accessToken: string) {
    return this.axiosInstance.delete<TableDataDeleteResponse>("/data/delete", {
      params: data,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
}
