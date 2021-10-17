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

interface IGetUserRequest {
	Id: number
}

interface IGetUserResponse {
	statusCode?: number,
	message?: string,
	userDto?: {
		id: number,
		email: string
	}
}

interface IDeleteUserRequest {
	UserId?: number
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
	private baseUrl: string = process.env.VUE_APP_API_BASEURL;

	static userAuthenticate(userData: IAuthenticateUserRequest): Promise<AxiosResponse<IAuthenticateUserResponse>> {
		return axios.post<IAuthenticateUserRequest, AxiosResponse<IAuthenticateUserResponse>>(
			`${process.env.VUE_APP_API_BASEURL}/auth/user/authenticate`, 
			{ email: userData.email, password: userData.password }
		)
	}

	static userCreate(userData: ICreateUserRequest): Promise<AxiosResponse<ICreateUserResponse>> {
		return axios.post<ICreateUserRequest, AxiosResponse<ICreateUserResponse>>(
			`${process.env.VUE_APP_API_BASEURL}/auth/user/create`, {
			email: userData.email,
			password: userData.password,
			confirmedPassword: userData.confirmedPassword
		})
	}

	static userDelete(): void {
		return;
	}

	static userGet(): void {
		return;
	}

	static userUpdate(): void {
		return;
	}

	static userRefreshToken(): Promise<AxiosResponse<IRefreshTokenResponse>> {
		return axios.post<IRefreshTokenResponse>(
			`${process.env.VUE_APP_API_BASEURL}/auth/user/refresh-token`
		)
	}

	static userRevokeToken(token: string): Promise<AxiosResponse<IRevokeTokenResponse>> {
		return axios.post<IRevokeTokenRequest, AxiosResponse<IRevokeTokenResponse>>(
			`${process.env.VUE_APP_API_BASEURL}/auth/user/revoke-token`,
			{ token: token }
		);
	}
}