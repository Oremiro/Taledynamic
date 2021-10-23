import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { SignInFormData, SignUpFormData } from '@/interfaces'
import { VueCookieNext } from 'vue-cookie-next'
import { ApiHelper } from '@/helpers/api'
import { AxiosError } from 'axios'

interface User {
	id: number | null,
	email: string
}

export interface State {
	user: User,
	accessTokenInMemory: string,
	pageStatus: 'loading' | 'ready' | 'error'
}


export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
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
		login(state: State, payload: { user: User, accessToken: string }) {
			state.user = payload.user;
			state.accessTokenInMemory = payload.accessToken;
		},
		logout(state: State) {
			state.user = { id: null, email: '' };
			state.accessTokenInMemory = '';
		},
		pageReady(state: State) {
			state.pageStatus = 'ready';
		},
		pageError(state: State) {
			state.pageStatus = 'error';
		},
		updateEmail(state: State, payload: { email: string }) {
			state.user.email = payload.email;
		}
  },
  actions: {
		async init(): Promise<boolean> {
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
		register(_context, formData: SignUpFormData): Promise<void> {
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
		login({ commit }, formData: SignInFormData): Promise<void> {
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
						const user: User = {
							id: response.data.id ?? null, 
							email: response.data.email ?? formData.email.value, 
						}
						commit('login', {
							user: user,
							accessToken: response.data.jwtToken
						});
						localStorage.setItem('user', JSON.stringify(user))
						resolve();
					} else {
						reject(new Error('Ошибка авторизации'));
					}
				})
				.catch(error => {
					if (error.response.status === 404) {
						reject(new Error('Пользователь с такими данными не найден'));
					} else {
						reject(new Error('Ошибка авторизации'));
					}
				});
			});
		},
		logout({ commit, state }): Promise<void> {
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
		refresh({ commit }): Promise<void> {			
			return new Promise<void>((resolve, reject) => {
				ApiHelper.userRefreshToken()
				.then((response) => {
					if (response.data.id && response.data.email && response.data.jwtToken) {
						const user: User = { id: response.data.id, email: response.data.email };
						const token: string = response.data.jwtToken
						commit('login', {
							user: user,
							accessToken: token
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
		},
		updateEmail({ commit, state }, email: string ): Promise<void> {
			return new Promise<void>((resolve, reject) => {
				if (state.user.id) {
					ApiHelper.userUpdate({ user: { id: state.user.id, email: email}}, state.accessTokenInMemory)
					.then((response) => {
						console.log(response);
						commit('updateEmail', { email: email });
						const user: User = {
							id: state.user.id, 
							email: email, 
						}
						localStorage.setItem('user', JSON.stringify(user))
						resolve();
					})
					.catch((error: AxiosError) => {
						console.log(error.message)
						console.log(error.response?.statusText);
						reject(new Error(error.message))
					})
				} else {
					reject(new Error('User id is null'));
				}
			})
		},
		updatePassword({ state }, data: { newPassword: string, confirmedPassword: string }): Promise<void> {
			return new Promise<void>((resolve, reject) => {
				if (state.user.id) {
					ApiHelper.userUpdate({ 
						user: { 
							id: state.user.id, 
							password: data.newPassword, 
							confirmPassword: data.confirmedPassword
						}
					}, state.accessTokenInMemory)
					.then(() => {
						resolve();
					})
					.catch((error: AxiosError) => {
						console.log(error.message)
						console.log(error.response?.statusText);
						reject(new Error(error.message))
					})
				} else {
					reject(new Error('User id is null'));
				}
			})
		}
  },
})

export function useStore(): Store<State> {
  return baseUseStore(key);
}