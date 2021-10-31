import { State } from "@/interfaces/store";

export const state: State = {
	user: {
		id: null,
		email: ''
	},
	accessTokenInMemory: '',
	pageStatus: 'loading'
};
