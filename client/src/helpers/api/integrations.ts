import axios from "axios";
import { IntegrationTelegramAuthorizeRequest, IntegrationTelegramRevokeRequest } from "@/models/api/requests";
import {
  IntegrationTelegramAuthorizeResponse,
  IntegrationTelegramGetResponse,
  IntegrationTelegramRevokeResponse
} from "@/models/api/responses";

export class IntegrationTelegramApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/integration/telegram",
    withCredentials: true
  });

  static async authorize(data: IntegrationTelegramAuthorizeRequest, accessToken: string) {
    return this.axiosInstance.post<IntegrationTelegramAuthorizeResponse>("/authorize", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
  static async get(accessToken: string) {
    return this.axiosInstance.get<IntegrationTelegramGetResponse>("get", {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
  static async revoke(data: IntegrationTelegramRevokeRequest, accessToken: string) {
    return this.axiosInstance.post<IntegrationTelegramRevokeResponse>("revoke", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
}
