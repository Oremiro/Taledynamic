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


export class ApiHelper {
	private static readonly baseUrl: string = process.env.VUE_APP_API_BASEURL;
	private static readonly axiosInstance = axios.create({
		baseURL: ApiHelper.baseUrl,
		withCredentials: true
	})

	static userAuthenticate(userData: IAuthenticateUserRequest): Promise<AxiosResponse<IAuthenticateUserResponse>> {
		return this.axiosInstance.post<IAuthenticateUserRequest, AxiosResponse<IAuthenticateUserResponse>>(
			'/auth/user/authenticate', 
			{ email: userData.email, password: userData.password },
		)
	}

	static userCreate(userData: ICreateUserRequest): Promise<AxiosResponse<ICreateUserResponse>> {
		return this.axiosInstance.post<ICreateUserRequest, AxiosResponse<ICreateUserResponse>>(
			'/auth/user/create', {
			email: userData.email,
			password: userData.password,
			confirmPassword: userData.confirmPassword
		})
	}

	static userDelete(userId: number, accessToken: string): Promise<AxiosResponse<IDeleteUserResponse>> {
		return this.axiosInstance.delete<IDeleteUserResponse>(
			'/auth/user/delete',
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
				params: { 
					userId: userId 
				}
			}
		)
	}

	static userGet(userId: number, accessToken: string): Promise<AxiosResponse<IGetUserResponse>> {
		return this.axiosInstance.get<IGetUserResponse>(
			'/auth/user/get', 
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				},
				params: {
					id: userId
				}
			}
		);
	}

	static userUpdate(userData: IUpdateUserRequest, accessToken: string): Promise<AxiosResponse<IUpdateUserResponse>> {
		return this.axiosInstance.put<IUpdateUserRequest, AxiosResponse<IUpdateUserResponse>>(
			'/auth/user/update',
			{ 
				id: userData.id, 
				email: userData.email, 
				password: userData.password, 
				confirmPassword: userData.confirmPassword 
			},
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

	static userRevokeToken(accessToken: string, token?: string, ): Promise<AxiosResponse<IRevokeTokenResponse>> {
		return this.axiosInstance.post<IRevokeTokenRequest, AxiosResponse<IRevokeTokenResponse>>(
			'/auth/user/revoke-token',
			token ? { token: token } : {},
			{
				headers: {
					'Authorization': `Bearer ${accessToken}`
				}
			}
		);
	}
}