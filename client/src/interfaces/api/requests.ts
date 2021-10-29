export interface AuthenticateUserRequest {
	email: string,
	password: string
}

export interface CreateUserRequest extends AuthenticateUserRequest {
	confirmPassword: string
}

export interface UpdateUserRequest {
	id: number,
	password: string,
	email?: string,
	newPassword?: string,
	confirmNewPassword?: string
}

export interface RevokeTokenRequest {
	token?: string
}
