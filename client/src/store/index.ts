import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { SignInFormData, SignUpFormData } from '@/interfaces'
import { VueCookieNext } from 'vue-cookie-next'
import { ApiHelper } from '@/helpers/api'
import { AxiosError } from 'axios'

interface IUser {
	id: number | null,
	email: string
}

export interface IState {
	user: IUser,
	accessTokenInMemory: string,
	pageStatus: 'loading' | 'ready' | 'error'
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
		pageStatus: 'loading'
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
		},
		pageReady(state: IState) {
			state.pageStatus = 'ready';
		},
		pageError(state: IState) {
			state.pageStatus = 'error';
		}
  },
  actions: {
		async init() {
			const isRemembered: string | null = VueCookieNext.getCookie('remembered');
			const localStorageUser: string | null = localStorage.getItem('user');
			if (isRemembered === '1' && localStorageUser) {
				try {
					await store.dispatch('refresh');
					return true;
				} catch (e) {
					VueCookieNext.removeCookie('remembered');
					localStorage.removeItem('user');
				}
			}
			return false;
		},
		register(_context, formData: SignUpFormData) {
			return new Promise<void>((resolve, reject) => {
				const newUser = {
					email: formData.email.value, 
					password: formData.password.value, 
					confirmPassword: formData.confirmedPassword.value
				}
				ApiHelper.userCreate({ user: newUser })
				.then((response) => {
					if (response.data.statusCode == 200) {
						resolve();
					} else {
						reject(new Error('Ошибка регистрации'))
					}
				}).catch((error: AxiosError) => {
					if (error.response?.status == 400) {
						reject(new Error('Пользователь с таким почтовым адресом уже существует'))
					} else {
						reject(new Error('Ошибка регистрации'));
					}
				});
			});
		},
		login({ commit }, formData: SignInFormData) {
			return new Promise<void>((resolve, reject) => {
				const expireValue: string = formData.remembered.value ? '7d' : '0';
				VueCookieNext.setCookie('remembered', '1', { expire: expireValue })
				const user = {
					email: formData.email.value, 
					password: formData.password.value
				}
				ApiHelper.userAuthenticate({ user: user })
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
				ApiHelper.userRevokeToken({}, state.accessTokenInMemory)
				.then((response) => {
					console.log(response.data);
					if (response.data.isSuccess) {
						commit('logout');
						VueCookieNext.removeCookie('remembered');
						localStorage.removeItem('user');
						resolve();
					} else {
						reject(new Error('Ошибка выхода из аккаунта'));
					}
				})
				.catch((error: AxiosError) => {
					if (error.response?.status == 404) {
						reject(new Error('Пользователь с таким токеном не найден'))	
					} else {
						reject(new Error('Ошибка обнуления токена'));
					}
				});
			});
		},
		refresh({ commit }) {			
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRefreshToken()
				.then((response) => {
					if (response.data.id && response.data.email && response.data.jwtToken) {
						const user: IUser = { id: response.data.id, email: response.data.email };
						const token: string = response.data.jwtToken
						commit('login', {
							user: user,
							token: token
						});
						resolve();
					} else {
						reject(new Error('Непредвиденная ошибка'))
					}
				})
				.catch((error: AxiosError) => {
					if(error.response?.status == 404) {
						reject(new Error('Сессия устарела'));
					} else {
						reject(new Error('Ошибка обновления токена'));
					}
				});
			});
		}
  },
})

export function useStore(): Store<IState> {
  return baseUseStore(key);
}