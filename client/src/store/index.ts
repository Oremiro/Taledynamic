import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { SignInFormData, SignUpFormData } from '@/interfaces'
import { useCookie } from 'vue-cookie-next'
import { ApiHelper } from '@/helpers/api'

interface IUser {
	id: number | null,
	email: string
}

export interface IState {
	user: IUser,
	accessToken: string,
	isLoggedIn: boolean
}

interface ILoginPayload {
	user: IUser,
	token: string
}

export const key: InjectionKey<Store<IState>> = Symbol()

export const store = createStore<IState>({
  state: {
    user: {
			id: null,
			email: ''
		},
		accessToken: '',
		isLoggedIn: false
  },
	mutations: {
		login(state: IState, payload: ILoginPayload) {
			state.user = payload.user;
			state.accessToken = payload.token;
			state.isLoggedIn = true;
		},
		logout(state: IState) {
			state.user = { id: null, email: '' };
			state.accessToken = '';
			state.isLoggedIn = false;
		}
  },
  actions: {
		register(_context, formData: SignUpFormData) {
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userCreate({
					email: formData.email.value, 
					password: formData.password.value, 
					confirmedPassword: formData.confirmedPassword.value
				})
				.then((response) => {
					console.log(response.data.message);
					resolve();
				}).catch((error) => {
					console.log(error.response);
					reject('Ошибка регистрации');
				});
			});
		},
		login({ commit }, formData: SignInFormData) {
			return new Promise<void>((resolve, reject) => {
				const cookie = useCookie();
				if (formData.remembered) {
					cookie.setCookie('remembered', '1', { expire: '7d'})
				} else {
					cookie.setCookie('remembered', '1', { expire: 0 })
				}
				ApiHelper.userAuthenticate({
					email: formData.email.value, 
					password: formData.password.value
				})
				.then((response) => {
					console.log(response);
					commit('login', {
						user: { email: formData.email.value, password: formData.password.value },
						token: response.data
					});
					resolve();
				})
				.catch((error) => {
					console.log(error.message);
					reject('Ошибка авторизации');
				});
			});
		},
		logout({ commit, state }) {
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRevokeToken(state.accessToken)
				.then((response) => {
					console.log(response.data);
					if (response.data.isSuccess) {
						commit('logout');
						resolve();
					} else {
						reject();
					}
				})
				.catch((error) => {
					console.log(error);
					reject();
				});
			});
		},
		refresh({ commit }) {			
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRefreshToken()
				.then((response) => {
					console.log(response.data);
					const token = response.data.jwtToken;
					const localStorageUser = localStorage.getItem('user');
					if (localStorageUser) {
						const user: IUser = JSON.parse(localStorageUser);
						commit('login', {
							user: user,
							token: token
						});
						resolve();
					} else {
						reject('User is none');
					}
				})
				.catch((error) => {
					console.log(error);
					reject('Refresh error');
				});
			});
		}
  },
})

export function useStore(): Store<IState> {
  return baseUseStore(key);
}