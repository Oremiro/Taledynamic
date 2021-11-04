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
	pageStatus: 'loading' | 'ready' | 'error'
}

export interface UserState {
	user: User,
	accessTokenInMemory: string,
}

export interface Workspace {
	id: number,
	name: string,
	created: Date,
	user: User
}

export interface WorkspacesState {
	workspaces: Workspace[],
	currentWorkspace: Workspace | null
}
