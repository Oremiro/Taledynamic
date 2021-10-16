import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import axios from 'axios'
import { SignInFormData, SignUpFormData } from '@/interfaces'

export interface User {
	id: number | null,
	email: string
}

export interface State {
	user: User,
	accessToken: string,
	loggedIn: boolean
}

export interface LoginPayload {
	user: User,
	token: string
}

export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  state: {
    user: {
			id: null,
			email: ''
		},
		accessToken: '',
		loggedIn: false
  },
	mutations: {
		login(state: State, payload: LoginPayload) {
			state.user = payload.user;
			state.accessToken = payload.token;
			state.loggedIn = true;
		},
		logout(state: State) {
			state.user = { id: null, email: '' };
			state.accessToken = '';
			state.loggedIn = false;
		}
  },
  actions: {
		register({ commit }, formData: SignUpFormData) {
			return new Promise<void>((resolve, reject) => {
				axios.post(`${process.env.VUE_APP_API_BASEURL}/auth/user/create`, {
					email: formData.email.value,
					password: formData.password.value,
					confirmPassword: formData.confirmedPassword.value
				}).then((response) => {
					console.log(response);
					commit('login', {
						user: { id: null, email: formData.email.value, password: formData.password.value },
						token: '???'
					});
					resolve();
				}).catch((error) => {
					console.log(error.response);
					reject('Ошибка регистрации');
				});
			});
		},
		login({ commit }, formData: SignInFormData) {
			return new Promise<void>((resolve, reject) => {
				axios.post(`${process.env.VUE_APP_API_BASEURL}/auth/user/authenticate`, {
					email: formData.email.value,
					password: formData.password.value
				}).then((response) => {
					console.log(response);
					commit('login', {
						user: { email: formData.email.value, password: formData.password.value },
						token: '???'
					});
					resolve();
				}).catch((error) => {
					console.log(error.message);
					reject('Ошибка авторизации');
				});
			});
		},
		logout({ commit }) {
			return new Promise<void>((resolve, reject) => {
				axios.post(
					`${process.env.VUE_APP_API_BASEURL}/auth/user/revoke-token`
				).then((response) => {
					console.log(response.data);
					commit('logout');
					resolve();
				}).catch((error) => {
					console.log(error);
					reject(new Error('Error'));
				});
			});
		}
  }
})

export function useStore(): Store<State> {
  return baseUseStore(key);
}