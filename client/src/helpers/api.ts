import axios, { AxiosResponse } from "axios";

interface User {
	id: number,
	email: string
}

interface BaseResponse {
	statusCode?: number,
	message?: string
}

interface AuthenticateUserRequest {
	email: string,
	password: string
}

interface AuthenticateUserResponse extends BaseResponse{
	id?: number,
	email?: string,
	jwtToken?: string
}

interface CreateUserRequest extends AuthenticateUserRequest {
	confirmPassword: string
}

interface CreateUserResponse extends BaseResponse {}

interface GetUserResponse extends BaseResponse {
	userDto?: User
}

interface UpdateUserRequest {
	id: number,
	password: string,
	email?: string,
	newPassword?: string,
	confirmNewPassword?: string
}

interface UpdateUserResponse extends BaseResponse {
	user: User
}

interface DeleteUserResponse extends BaseResponse {}

interface RefreshTokenResponse extends BaseResponse {
	id?: number,
	email?: string,
	jwtToken?: string
}

interface RevokeTokenRequest {
	token?: string
}

interface RevokeTokenResponse extends BaseResponse {
	isSuccess?: boolean
}

interface GetByEmailResponse extends BaseResponse {
	user: User
}

interface IsEmailUsedResponse extends BaseResponse {
	isEmailUsed: boolean
}


export class ApiHelper {
	private static readonly baseUrl: string = process.env.VUE_APP_API_BASEURL;
	private static readonly axiosInstance = axios.create({
		baseURL: ApiHelper.baseUrl,
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