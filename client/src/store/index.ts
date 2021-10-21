import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { SignInFormData, SignUpFormData } from '@/interfaces'
import { VueCookieNext } from 'vue-cookie-next'
import { ApiHelper } from '@/helpers/api'

interface IUser {
	id: number | null,
	email: string
}

export interface IState {
	user: IUser,
	accessTokenInMemory: string,
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
		accessTokenInMemory: '',
  },
	getters: {
		isLoggedIn(state): boolean {
			return state.accessTokenInMemory != '';
		}
	},
	mutations: {
		login(state: IState, payload: ILoginPayload) {
			state.user = payload.user;
			state.accessTokenInMemory = payload.token;
		},
		logout(state: IState) {
			state.user = { id: null, email: '' };
			state.accessTokenInMemory = '';
		}
  },
  actions: {
		register(_context, formData: SignUpFormData) {
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userCreate({
					email: formData.email.value, 
					password: formData.password.value, 
					confirmPassword: formData.confirmedPassword.value
				})
				.then((response) => {
					if (response.data.statusCode == 200) {
						resolve();
					} else if (response.data.statusCode == 400) {
						reject(new Error('Пользователь с таким почтовым адресом уже существует'))
					} else {
						reject(new Error('Ошибка регистрации'))
					}
				}).catch((error) => {
					console.log(error.response);
					reject(new Error('Ошибка регистрации'));
				});
			});
		},
		login({ commit }, formData: SignInFormData) {
			return new Promise<void>((resolve, reject) => {
				const expireValue: string = formData.remembered.value ? '7d' : '0';
				VueCookieNext.setCookie('remembered', '1', { expire: expireValue })
				ApiHelper.userAuthenticate({
					email: formData.email.value, 
					password: formData.password.value
				})
				.then((response) => {
					if (response.data.statusCode == 200) {
						const user: IUser = {
							id: response.data.id ?? null, 
							email: response.data.email ?? formData.email.value, 
						}
						commit('login', {
							user: user,
							token: response.data.jwtToken
						});
						localStorage.setItem('user', JSON.stringify(user))
						resolve();
					} else if (response.data.statusCode == 404) {
						reject(new Error('Пользователь с такими данными не найден'));
					} else {
						reject(new Error('Ошибка авторизации'));
					}
				})
				.catch((error) => {
					console.log(error.message);
					reject(new Error('Ошибка авторизации'));
				});
			});
		},
		logout({ commit, state }) {
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRevokeToken(state.accessTokenInMemory)
				.then((response) => {
					console.log(response.data);
					if (response.data.isSuccess) {
						commit('logout');
						localStorage.removeItem('user');
						resolve();
					} else {
						reject();
					}
				})
				.catch((error) => {
					console.log(error);
					reject('Ошибка выхода');
				});
			});
		},
		refresh({ commit }) {			
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRefreshToken()
				.then((response) => {
					console.log(response);
					if (response.data.statusCode == 200) {
						const localStorageUser = localStorage.getItem('user');
						if (localStorageUser) {
							const user: IUser = JSON.parse(localStorageUser);
							commit('login', {
								user: user,
								token: response.data.jwtToken
							});
							resolve();
						} else {
							reject(new Error('Пользователь не найден в локальном хранилище'));
						}
					} else if (response.data.statusCode == 404) {
						console.log(response)
						reject(new Error('Сессия устарела'));
					} else {
						console.log(response)
						reject(new Error('Непредвиденная ошибка'));
					}
				})
				.catch((error) => {
					console.log(error.response);
					reject('Ошибка обновления токена');
				});
			});
		}
  },
})

export function useStore(): Store<IState> {
  return baseUseStore(key);
}