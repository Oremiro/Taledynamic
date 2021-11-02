import { Module } from 'vuex'
import { state } from '@/store/modules/workspaces/state'
import { State, WorkspacesState } from '@/interfaces/store'


export default {
	namespaced: true,
	state
} as Module<WorkspacesState, State>