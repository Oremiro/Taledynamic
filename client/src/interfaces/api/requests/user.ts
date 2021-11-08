export interface AuthenticateUserRequest {
	email: string,
	password: string
}

export interface CreateUserRequest extends AuthenticateUserRequest {
	confirmPassword: string
}

export interface DeleteUserRequest {
	userId: number
}

export interface GetUserRequest {
	id: number
}

export interface UpdateUserRequest {
	id: number,
	password: string,
	email?: string,
	newPassword?: string,
	confirmNewPassword?: string
}

export interface RevokeTokenRequest {
	refreshToken?: string
}

export interface GetByEmailRequest {
	email: string
}

export interface IsEmailUsedRequest extends GetByEmailRequest {}