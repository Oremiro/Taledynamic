import { AuthenticateUserRequest, CreateUserRequest, UpdateUserRequest } from "@/interfaces/api/requests";
import { AuthenticateUserResponse, CreateUserResponse, DeleteUserResponse, GetUserResponse, UpdateUserResponse, 
	RefreshTokenResponse, RevokeTokenResponse, GetByEmailResponse, IsEmailUsedResponse } from "@/interfaces/api/responses";
import { AxiosResponse } from "axios";
import { Api } from "@/helpers/api/base";


export class UserApi extends Api {
	static authenticate(data: { user: AuthenticateUserRequest }): Promise<AxiosResponse<AuthenticateUserResponse>> {
		return this.axiosInstance.post<AuthenticateUserResponse>(
			'/auth/user/authenticate', 
			data.user
		);
	}

	static create(data: { user: CreateUserRequest }): Promise<AxiosResponse<CreateUserResponse>> {
		return this.axiosInstance.post<CreateUserResponse>(
			'/auth/user/create', 
			data.user
		)
	}


	static delete(data: { userId: number }, accessToken: string): Promise<AxiosResponse<DeleteUserResponse>> {
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

	static get(data: { userId: number }, accessToken: string): Promise<AxiosResponse<GetUserResponse>> {
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

	static update(data: { user: UpdateUserRequest }, accessToken: string): Promise<AxiosResponse<UpdateUserResponse>> {
		return this.axiosInstance.put<UpdateUserResponse>(
			'/auth/user/update',
			data.user,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
			}
		);
	}

	static refreshToken(): Promise<AxiosResponse<RefreshTokenResponse>> {
		return this.axiosInstance.post<RefreshTokenResponse>(
			'/auth/user/refresh-token'
		)
	}

	static revokeToken(data: { token?: string }, accessToken: string): Promise<AxiosResponse<RevokeTokenResponse>> {
		return this.axiosInstance.post<RevokeTokenResponse>(
			'/auth/user/revoke-token',
			data,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				}
			}
		);
	}

	static getByEmail(data: { email: string }): Promise<AxiosResponse<GetByEmailResponse>> {
		return this.axiosInstance.get<GetByEmailResponse>(
			'/auth/user/get-by-email',
			{
				params: {
					email: data.email
				}
			}
		)
	}

	static isEmailUsed(data: { email: string }): Promise<AxiosResponse<IsEmailUsedResponse>> {
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