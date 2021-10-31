export interface User {
	id: number | null,
	email: string
}

interface UpdatedBaseData {
	currentPassword: string
} 

export interface UpdatedEmailData extends UpdatedBaseData  {
	newEmail: string
}

export interface UpdatedPasswordData extends UpdatedBaseData {
	newPassword: string,
	confirmedNewPassword: string
}

export interface State {
	user: User,
	accessTokenInMemory: string,
	pageStatus: 'loading' | 'ready' | 'error'
}