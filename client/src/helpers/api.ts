import axios, { AxiosResponse } from "axios";

interface IAuthenticateUserRequest {
	email: string,
	password: string
}

interface IAuthenticateUserResponse {
	statusCode?: number,
	message?: string,
	id?: number,
	email?: string,
	jwtToken?: string
}

interface ICreateUserRequest extends IAuthenticateUserRequest {
	confirmedPassword: string
}

interface ICreateUserResponse {
	statusCode?: number,
	message?: string
}

interface IGetUserResponse {
	statusCode?: number,
	message?: string,
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

interface IUpdateUserResponse {
	statusCode?: number,
	message?: string
}

interface IDeleteUserResponse {
	statusCode?: number,
	message?: string
}

interface IRefreshTokenResponse {
	statusCode?: number,
	message?: string,
	id?: number,
	email?: string,
	jwtToken?: string
}

interface IRevokeTokenRequest {
	token?: string
}

interface IRevokeTokenResponse {
	statusCode?: number,
	message?: string,
	isSuccess?: boolean
}


export class ApiHelper {
	private static baseUrl: string = process.env.VUE_APP_API_BASEURL;

	static userAuthenticate(userData: IAuthenticateUserRequest): Promise<AxiosResponse<IAuthenticateUserResponse>> {
		return axios.post<IAuthenticateUserRequest, AxiosResponse<IAuthenticateUserResponse>>(
			`${this.baseUrl}/auth/user/authenticate`, 
			{ email: userData.email, password: userData.password }
		)
	}

	static userCreate(userData: ICreateUserRequest): Promise<AxiosResponse<ICreateUserResponse>> {
		return axios.post<ICreateUserRequest, AxiosResponse<ICreateUserResponse>>(
			`${this.baseUrl}/auth/user/create`, {
			email: userData.email,
			password: userData.password,
			confirmedPassword: userData.confirmedPassword
		})
	}

	static userDelete(userId: number): Promise<AxiosResponse<IDeleteUserResponse>> {
		return axios.delete<IDeleteUserResponse>(
			`${this.baseUrl}/auth/user/delete`,
			{
				params: { 
					UserId: userId 
				}
			}
		)
	}

	static userGet(userId: number): Promise<AxiosResponse<IGetUserResponse>> {
		return axios.get<IGetUserResponse>(
			`${this.baseUrl}/auth/user/get`, 
			{
				params: {
					Id: userId
				}
			}
		);
	}

	static userUpdate(userData: IUpdateUserRequest): Promise<AxiosResponse<IUpdateUserResponse>> {
		return axios.put<IUpdateUserRequest, AxiosResponse<IUpdateUserResponse>>(
			`${this.baseUrl}/auth/user/update`,
			{ 
				id: userData.id, 
				email: userData.email, 
				password: userData.password, 
				confirmPassword: userData.confirmPassword 
			}
		);
	}

	static userRefreshToken(): Promise<AxiosResponse<IRefreshTokenResponse>> {
		return axios.post<IRefreshTokenResponse>(
			`${this.baseUrl}/auth/user/refresh-token`
		)
	}

	static userRevokeToken(token: string): Promise<AxiosResponse<IRevokeTokenResponse>> {
		return axios.post<IRevokeTokenRequest, AxiosResponse<IRevokeTokenResponse>>(
			`${this.baseUrl}/auth/user/revoke-token`,
			{ token: token }
		);
	}
}