import axios, { AxiosResponse } from "axios";

interface IBaseResponse {
	statusCode?: number,
	message?: string
}

interface IAuthenticateUserRequest {
	email: string,
	password: string
}

interface IAuthenticateUserResponse extends IBaseResponse{
	id?: number,
	email?: string,
	jwtToken?: string
}

interface ICreateUserRequest extends IAuthenticateUserRequest {
	confirmPassword: string
}

interface ICreateUserResponse extends IBaseResponse {}

interface IGetUserResponse extends IBaseResponse {
	userDto?: {
		id: number,
		email: string
	}
}

interface IUpdateUserRequest {
	id: number,
	email?: string,
	password?: string,
	confirmPassword?: string
}

interface IUpdateUserResponse extends IBaseResponse {}

interface IDeleteUserResponse extends IBaseResponse {}

interface IRefreshTokenResponse extends IBaseResponse {
	id?: number,
	email?: string,
	jwtToken?: string
}

interface IRevokeTokenRequest {
	token?: string
}

interface IRevokeTokenResponse extends IBaseResponse {
	isSuccess?: boolean
}

interface IIsEmailUsedResponse extends IBaseResponse {
	isEmailUsed: boolean
}


export class ApiHelper {
	private static readonly baseUrl: string = process.env.VUE_APP_API_BASEURL;
	private static readonly axiosInstance = axios.create({
		baseURL: ApiHelper.baseUrl,
		withCredentials: true
	})

	static userAuthenticate(data: { user: IAuthenticateUserRequest }): Promise<AxiosResponse<IAuthenticateUserResponse>> {
		return this.axiosInstance.post<IAuthenticateUserRequest, AxiosResponse<IAuthenticateUserResponse>>(
			'/auth/user/authenticate', 
			data.user,
		)
	}

	static userCreate(data: { user: ICreateUserRequest }): Promise<AxiosResponse<ICreateUserResponse>> {
		return this.axiosInstance.post<ICreateUserRequest, AxiosResponse<ICreateUserResponse>>(
			'/auth/user/create', 
			data.user
		)
	}

	static userDelete(data: { userId: number }, accessToken: string): Promise<AxiosResponse<IDeleteUserResponse>> {
		return this.axiosInstance.delete<IDeleteUserResponse>(
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

	static userGet(data: { userId: number }, accessToken: string): Promise<AxiosResponse<IGetUserResponse>> {
		return this.axiosInstance.get<IGetUserResponse>(
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

	static userUpdate(data: { user: IUpdateUserRequest }, accessToken: string): Promise<AxiosResponse<IUpdateUserResponse>> {
		return this.axiosInstance.put<IUpdateUserRequest, AxiosResponse<IUpdateUserResponse>>(
			'/auth/user/update',
			data.user,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
			}
		);
	}

	static userRefreshToken(): Promise<AxiosResponse<IRefreshTokenResponse>> {
		return this.axiosInstance.post<IRefreshTokenResponse>(
			'/auth/user/refresh-token'
		)
	}

	static userRevokeToken(data: { token?: string }, accessToken: string): Promise<AxiosResponse<IRevokeTokenResponse>> {
		return this.axiosInstance.post<IRevokeTokenRequest, AxiosResponse<IRevokeTokenResponse>>(
			'/auth/user/revoke-token',
			data,
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				}
			}
		);
	}

	static isEmailUsed(data: { email: string }): Promise<AxiosResponse<IIsEmailUsedResponse>> {
		return this.axiosInstance.get<IIsEmailUsedResponse>(
			'/auth/User/is-email-used',
			{
				params: {
					email: data.email
				}
			}
		)
	}
}