import { FileCreateRequest, FileGetFileRequest, FileGetLinkRequest } from "@/models/api/requests";
import { FileCreateResponse, FileGetFileResponse, FileGetLinkResponse } from "@/models/api/responses";
import axios from "axios";

export class FileApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/data/file",
    withCredentials: true
  });

  static async create(data: FileCreateRequest, accessToken: string) {
    return this.axiosInstance.post<FileCreateResponse>("/create", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static async getLink(data: FileGetLinkRequest, accessToken: string) {
    return this.axiosInstance.get<FileGetLinkResponse>("/get/link", {
      params: data,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }

  static async getFile(data: FileGetFileRequest, accessToken: string) {
    return this.axiosInstance.get<FileGetFileResponse>("/get/file", {
      params: data,
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    }); 
  }
}
