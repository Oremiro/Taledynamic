import BaseResponse from '@/interfaces/api/responses/base'

interface User {
	id: number,
	email: string
}

export interface AuthenticateUserResponse extends BaseResponse {
	id?: number,
	email?: string,
	jwtToken?: string
}

export interface CreateUserResponse extends BaseResponse {}

export interface GetUserResponse extends BaseResponse {
	userDto?: User
}

export interface UpdateUserResponse extends BaseResponse {
	user: User
}

export interface DeleteUserResponse extends BaseResponse {}

export interface RefreshTokenResponse extends BaseResponse {
	id?: number,
	email?: string,
	jwtToken?: string
}

export interface RevokeTokenResponse extends BaseResponse {
	isSuccess?: boolean
}

export interface GetByEmailResponse extends BaseResponse {
	user: User
}

export interface IsEmailUsedResponse extends BaseResponse {
	isEmailUsed: boolean
}
