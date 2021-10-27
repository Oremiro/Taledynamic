import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { SignInFormData, SignUpFormData } from '@/interfaces'
import { VueCookieNext } from 'vue-cookie-next'
import { ApiHelper } from '@/helpers/api'
import axios from 'axios'

interface User {
	id: number | null,
	email: string
}

interface UpdatedBaseData {
	currentPassword: string
} 

interface UpdatedEmailData extends UpdatedBaseData  {
	newEmail: string
}

interface UpdatedPasswordData extends UpdatedBaseData {
	newPassword: string,
	confirmedNewPassword: string
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
		setUser(state: State, payload: { user: User }) {
			state.user = payload.user;
		},
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
		}
  },
  actions: {
		async init(): Promise<boolean> {
			const isRemembered: string | null = VueCookieNext.getCookie('remembered');
			const localStorageUser: string | null = localStorage.getItem('user');
			try {
				if (isRemembered === '1' && localStorageUser) {
					await store.dispatch('refresh');
					return true;
				} else {
					throw new Error('Empty data to continue session')
				}
			} catch (error) {
				VueCookieNext.removeCookie('remembered');
				localStorage.removeItem('user');
			}
			return false;
		},
		async register(_context, formData: SignUpFormData): Promise<void> {
			const newUser = {
				email: formData.email.value, 
				password: formData.password.value, 
				confirmPassword: formData.confirmedPassword.value
			}
			try {
				const { data } = await ApiHelper.userCreate({ user: newUser });
				if (data.statusCode === 200) {
					return;
				} else {
					throw new Error('Ошибка регистрации')
				}
			} catch (error) {
				if(axios.isAxiosError(error)) {
					if (error.response?.status == 400) {
						throw new Error('Пользователь с таким почтовым адресом уже существует');
					} else {
						throw new Error('Ошибка регистрации');
					}
				}
			}
		},
		async login({ commit }, formData: SignInFormData): Promise<void> {
			const requestedUser = {
				email: formData.email.value, 
				password: formData.password.value
			}
			try {
				const { data } = await ApiHelper.userAuthenticate({ user: requestedUser });
				if (data.statusCode === 200) {
					const responsedUser: User = {
						id: data.id ?? null, 
						email: data.email ?? formData.email.value, 
					}
					commit('login', {
						user: responsedUser,
						accessToken: data.jwtToken
					});
					const expireValue: string = formData.remembered.value ? '7d' : '0';
					VueCookieNext.setCookie('remembered', '1', { expire: expireValue })
					localStorage.setItem('user', JSON.stringify(responsedUser))
				} else {
					throw new Error('Ошибка авторизации');
				}
			} catch (error) {
				if(axios.isAxiosError(error)) {
					if (error.response?.status === 404) {
						throw new Error('Пользователь с такими данными не найден');
					} else {
						throw new Error('Ошибка авторизации');
					}
				}
			}
		},
		async logout({ commit, state }): Promise<void> {
			try {
				const { data } = await ApiHelper.userRevokeToken({}, state.accessTokenInMemory)
				if (data.isSuccess && data.statusCode === 200) {
					commit('logout');
					VueCookieNext.removeCookie('remembered');
					localStorage.removeItem('user');
					return;
				} else {
					throw new Error('Ошибка при выходе из аккаунта');
				}
			} catch (error) {
				if (axios.isAxiosError(error)) {
					if (error.response?.status === 404) {
						throw new Error('Пользователь с таким токеном не найден');	
					} else {
						throw new Error('Ошибка при обнулении токена');
					}		
				}
			}
		},
		async refresh({ commit }): Promise<void> {			
			try {
				const { data } = await ApiHelper.userRefreshToken();
				if (data.id && data.email && data.jwtToken) {
					const user: User = { id: data.id, email: data.email };
					const token: string = data.jwtToken
					commit('login', {
						user: user,
						accessToken: token
					});
					localStorage.setItem('user', JSON.stringify(user));
					return;
				} else {
					throw new Error('Непредвиденная ошибка');
				}
			}	catch (error) {
				if (axios.isAxiosError(error)) {
					if (error.response?.status === 404) {
						throw new Error('Сессия устарела');
					} else {
						throw new Error('Ошибка обновления токена');
					}
				}
			}
		},
		async updateEmail({ commit, state }, data: UpdatedEmailData): Promise<void> {
			if (state.user.id) {
				try {
					const { data: responseData } = await ApiHelper.userUpdate({ 
						user: { 
							id: state.user.id, 
							password: data.currentPassword, 
							email: data.newEmail
						}
					}, state.accessTokenInMemory);
					const user: User = {
						id: responseData.user.id, 
						email: responseData.user.email, 
					}
					commit('setUser', { user });
					localStorage.setItem('user', JSON.stringify(user))
					return;
				} catch (error) {
					if (axios.isAxiosError(error)) {
						if (error.response?.status === 404) {
							throw new Error('Неправильный текущий пароль')
						} else if (error.response?.status === 400) {
							throw new Error('Данный email занят другим пользователем');
						} else {
							throw new Error(error.message);
						}
					}
				}
			} else {
				throw new Error('Отсутствует ID пользователя');
			}
		},
		async updatePassword({ commit, state }, data: UpdatedPasswordData): Promise<void> {
			if (state.user.id) {
				try {
					const { data: responseData } = await ApiHelper.userUpdate({ 
						user: { 
							id: state.user.id, 
							password: data.currentPassword,
							newPassword: data.newPassword, 
							confirmNewPassword: data.confirmedNewPassword
						}
					}, state.accessTokenInMemory)
					const user: User = {
						id: responseData.user.id, 
						email: responseData.user.email, 
					}
					commit('setUser', { user });
					localStorage.setItem('user', JSON.stringify(user))
					return;
				} catch (error) {
					if (axios.isAxiosError(error)) {
						if (error.response?.status === 404) {
							throw new Error('Неправильный текущий пароль')
						} else if (error.response?.status === 400) {
							throw new Error('Пароли не совпадают');
						} else {
							throw new Error(error.message);
						}
					}
				}
			} else {
				throw new Error('Отсутствует ID пользователя');
			}
		},
		async delete({ commit, state }): Promise<void> {
			if (state.user.id) {
				try {
					await ApiHelper.userDelete({ userId: state.user.id }, state.accessTokenInMemory);
					commit('logout');
					VueCookieNext.removeCookie('remembered');
					localStorage.removeItem('user');
					return;
				} catch (error) {
					if (axios.isAxiosError(error)) {
						if (error.response?.status === 404) {
							throw new Error('?');
						} else if (error.response?.status === 400) {
							throw new Error('Неверный ID пользователя')
						} else {
							throw new Error(error.message);
						}
					}
				}
				return;
			} else {
				throw new Error('Отсутствует ID пользователя');
			}
		}
  },
})

export function useStore(): Store<State> {
  return baseUseStore(key);
}