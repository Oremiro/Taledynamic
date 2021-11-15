import {
  AuthenticateUserRequest,
  CreateUserRequest,
  DeleteUserRequest,
  GetByEmailRequest,
  GetUserRequest,
  IsEmailUsedRequest,
  RevokeTokenRequest,
  UpdateUserRequest,
} from "@/interfaces/api/requests";
import {
  AuthenticateUserResponse,
  CreateUserResponse,
  DeleteUserResponse,
  GetUserResponse,
  UpdateUserResponse,
  RefreshTokenResponse,
  RevokeTokenResponse,
  GetByEmailResponse,
  IsEmailUsedResponse,
} from "@/interfaces/api/responses";
import axios, { AxiosPromise } from "axios";

export class UserApi {
  private static readonly axiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASEURL + "/auth/user",
    withCredentials: true,
  });

  static authenticate(
    data: AuthenticateUserRequest
  ): AxiosPromise<AuthenticateUserResponse> {
    return this.axiosInstance.post<AuthenticateUserResponse>(
      "/authenticate",
      data
    );
  }

  static create(data: CreateUserRequest): AxiosPromise<CreateUserResponse> {
    return this.axiosInstance.post<CreateUserResponse>("/create", data);
  }

  static delete(
    params: DeleteUserRequest,
    accessToken: string
  ): AxiosPromise<DeleteUserResponse> {
    return this.axiosInstance.delete<DeleteUserResponse>("/delete", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  static get(
    params: GetUserRequest,
    accessToken: string
  ): AxiosPromise<GetUserResponse> {
    return this.axiosInstance.get<GetUserResponse>("/get", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  static update(
    data: UpdateUserRequest,
    accessToken: string
  ): AxiosPromise<UpdateUserResponse> {
    return this.axiosInstance.put<UpdateUserResponse>("/update", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  static refreshToken(): AxiosPromise<RefreshTokenResponse> {
    return this.axiosInstance.post<RefreshTokenResponse>("/refresh-token");
  }

  static revokeToken(
    data: RevokeTokenRequest,
    accessToken: string
  ): AxiosPromise<RevokeTokenResponse> {
    return this.axiosInstance.post<RevokeTokenResponse>("/revoke-token", data, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  static getByEmail(
    params: GetByEmailRequest,
    accessToken: string
  ): AxiosPromise<GetByEmailResponse> {
    return this.axiosInstance.get<GetByEmailResponse>("/get-by-email", {
      params,
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });
  }

  static isEmailUsed(
    params: IsEmailUsedRequest
  ): AxiosPromise<IsEmailUsedResponse> {
    return this.axiosInstance.get<IsEmailUsedResponse>("/is-email-used", {
      params,
    });
  }
}
