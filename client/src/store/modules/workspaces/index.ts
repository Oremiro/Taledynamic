import { Module } from 'vuex'
import { state } from '@/store/modules/workspaces/state'
import { getters } from '@/store/modules/workspaces/getters'
import { State, WorkspacesState } from '@/interfaces/store'


export default {
	namespaced: true,
	state,
	getters
} as Module<WorkspacesState, State>