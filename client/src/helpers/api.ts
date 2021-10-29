import { AuthenticateUserRequest, CreateUserRequest, UpdateUserRequest, RevokeTokenRequest } from "@/interfaces/api/requests/user";
import { AuthenticateUserResponse, CreateUserResponse, DeleteUserResponse, GetUserResponse, UpdateUserResponse, 
	RefreshTokenResponse, RevokeTokenResponse, GetByEmailResponse, IsEmailUsedResponse } from "@/interfaces/api/responses/user";
import axios, { AxiosResponse } from "axios";

export class Api {
	private static readonly baseUrl: string = process.env.VUE_APP_API_BASEURL;
	private static readonly axiosInstance = axios.create({
		baseURL: Api.baseUrl,
		withCredentials: true
	})

	static userAuthenticate(data: { user: AuthenticateUserRequest }): Promise<AxiosResponse<AuthenticateUserResponse>> {
		return this.axiosInstance.post<AuthenticateUserRequest, AxiosResponse<AuthenticateUserResponse>>(
			'/auth/user/authenticate', 
			data.user
		);
	}

	static userCreate(data: { user: CreateUserRequest }): Promise<AxiosResponse<CreateUserResponse>> {
		return this.axiosInstance.post<CreateUserRequest, AxiosResponse<CreateUserResponse>>(
			'/auth/user/create', 
			data.user
		)
	}

	static userDelete(data: { userId: number }, accessToken: string): Promise<AxiosResponse<DeleteUserResponse>> {
		return this.axiosInstance.delete<DeleteUserResponse>(
			'/auth/user/delete',
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
				params: { 
					userId: data.userId 
				}
			}
		)
	}

	static userGet(data: { userId: number }, accessToken: string): Promise<AxiosResponse<GetUserResponse>> {
		return this.axiosInstance.get<GetUserResponse>(
			'/auth/user/get', 
			{
				params: {
					id: data.userId
				},
				headers: {
					'Authorization': `Bearer ${accessToken}`
				}
			}
		);
	}

	static userUpdate(data: { user: UpdateUserRequest }, accessToken: string): Promise<AxiosResponse<UpdateUserResponse>> {
		return this.axiosInstance.put<UpdateUserRequest, AxiosResponse<UpdateUserResponse>>(
			'/auth/user/update',
			data.user,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
			}
		);
	}

	static userRefreshToken(): Promise<AxiosResponse<RefreshTokenResponse>> {
		return this.axiosInstance.post<RefreshTokenResponse>(
			'/auth/user/refresh-token'
		)
	}

	static userRevokeToken(data: { token?: string }, accessToken: string): Promise<AxiosResponse<RevokeTokenResponse>> {
		return this.axiosInstance.post<RevokeTokenRequest, AxiosResponse<RevokeTokenResponse>>(
			'/auth/user/revoke-token',
			data,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				}
			}
		);
	}

	static userGetByEmail(data: { email: string }): Promise<AxiosResponse<GetByEmailResponse>> {
		return this.axiosInstance.get<GetByEmailResponse>(
			'/auth/user/get-by-email',
			{
				params: {
					email: data.email
				}
			}
		)
	}

	static userIsEmailUsed(data: { email: string }): Promise<AxiosResponse<IsEmailUsedResponse>> {
		return this.axiosInstance.get<IsEmailUsedResponse>(
			'/auth/user/is-email-used',
			{
				params: {
					email: data.email
				}
			}
		)
	}
}