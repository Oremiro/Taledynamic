import { InjectionKey } from 'vue'
import { createStore, useStore as baseUseStore, Store } from 'vuex'
import { State } from '@/interfaces/store'
import { state } from '@/store/state'
import { getters } from '@/store/getters'
import { mutations } from '@/store/mutations'
import { actions } from '@/store/actions'

export const key: InjectionKey<Store<State>> = Symbol()

export const store = createStore<State>({
  state: state,
	getters: getters,
	mutations: mutations,
  actions: actions
})

export function useStore(): Store<State> {
  return baseUseStore(key);
}