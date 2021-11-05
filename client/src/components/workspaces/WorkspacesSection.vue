<template>
	<transition name="fade" mode="out-in">
		<workspace-loading v-if="isListLoading" :error="isListLoadingError" @repeat-loading="getWorkspaces" />
		<div v-else class="workspaces-section-content">
			<div class="workspaces-section-content-header">
				<n-text depth="3">Ваши рабочие пространства</n-text>
				<n-popselect trigger="click" :options="popOptions" v-model:value="popSortValue" @update:value="updateHandler">
					<n-button text>
						<n-icon size="1.2rem">
							<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M17.25 4l-.1.007a.75.75 0 0 0-.65.743v12.692l-3.22-3.218l-.084-.072a.75.75 0 0 0-.976 1.134l4.504 4.5l.084.072a.75.75 0 0 0 .976-.073l4.497-4.5l.072-.084a.75.75 0 0 0-.073-.977l-.084-.072a.75.75 0 0 0-.977.073L18 17.446V4.75l-.006-.102A.75.75 0 0 0 17.251 4zm-11.036.22L1.72 8.715l-.073.084a.75.75 0 0 0 .073.976l.084.073a.75.75 0 0 0 .976-.073l3.217-3.218v12.698l.008.102a.75.75 0 0 0 .743.648l.101-.007a.75.75 0 0 0 .649-.743L7.497 6.559l3.223 3.217l.084.072a.75.75 0 0 0 .975-1.134L7.275 4.22l-.085-.072a.75.75 0 0 0-.976.073z" fill="currentColor"></path></g></svg>
						</n-icon>
					</n-button>
				</n-popselect>
			</div>
			<n-scrollbar style="height: 100%;">
				<workspaces-list :workspaces="workspaces" />
			</n-scrollbar>
			<div class="workspaces-section-content-footer">
				<workspace-creating-button />
			</div>
		</div>
	</transition>
</template>

<style lang="scss" scoped>
.workspaces-section-content {
	height: 100%; 
	display: flex; 
	flex-direction: column;
}

.workspaces-section-content-header {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: space-between;
}
.workspaces-section-content-footer {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: center;
}
</style>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { SelectGroupOption, SelectOption, NPopselect, NScrollbar } from 'naive-ui';
import { useStore } from '@/store';
import { Workspace } from '@/interfaces/store';
import WorkspaceCreatingButton from '@/components/workspaces/WorkspaceCreating.vue';
import WorkspacesList from '@/components/workspaces/WorkspacesList.vue'
import WorkspaceLoading from '@/components/workspaces/WorkspacesLoading.vue'

// data
const store = useStore();
// TODO
const workspaces = computed<Workspace[]>(() => store.getters['workspaces/workspaces'])
const popOptions: Array<SelectOption | SelectGroupOption> = [
	{
		type: 'group',
		label: 'По дате',
		key: 'sortByDate',
		children: [
			{
				label: 'Сначала новые',
				value: 'sortDescendingByDate'
			},
			{
				label: 'Сначала старые',
				value: 'sortAscendingByDate'
			},
		]
	},
	{
		type: 'group',
		label: 'По названию',
		key: 'sortByName',
		children: [
			{
				label: 'От A до Z',
				value: 'sortAscendingByName'
			},
			{
				label: 'От Z до A',
				value: 'sortDescendingByName'
			},
		]
	}
]

const popSortValue = ref<string>(localStorage.getItem('workspacesSort') ?? 'sortAscendingByDate');

const isListLoading = ref<boolean>(true);
const isListLoadingError = ref<boolean>(false);

// methods
const sortWorkspacesListByName = (reverse = false): void => {
	workspaces.value.sort((itemFirst, itemSecond) => {
		if (itemFirst.name > itemSecond.name) {
			return reverse ? -1 : 1;
		} else if (itemFirst.name < itemSecond.name) {
			return reverse ? 1 : -1;
		}
		return 0;
	})
}
const sortWorkspacesListByDate = (reverse = false): void => {
	workspaces.value.sort((itemFirst, itemSecond) => reverse ? 
		itemSecond.modified.getTime() - itemFirst.modified.getTime() : 
		itemFirst.modified.getTime() - itemSecond.modified.getTime())
}
const sortWorkspacesList = (value: string): void => {
	switch (value) {
		case 'sortAscendingByName': {
			sortWorkspacesListByName();
			break;
		}
		case 'sortDescendingByName': {
			sortWorkspacesListByName(true);
			break;
		}
		case 'sortAscendingByDate': {
			sortWorkspacesListByDate();
			break;
		}
		case 'sortDescendingByDate': {
			sortWorkspacesListByDate(true);
			break;
		}
	}
}
const updateHandler = (value: string): void => {
	localStorage.setItem('workspacesSort', value);
	sortWorkspacesList(value);
}

async function getWorkspaces(): Promise<void> {
	isListLoadingError.value = false;
	try {
		await store.dispatch('workspaces/init');
		isListLoading.value = false;
	} catch (error) {
		if (error instanceof Error) {
			isListLoadingError.value = true;
		}
	}
}

onMounted((): void => {
	setTimeout(async (): Promise<void> => {
		await getWorkspaces();
		sortWorkspacesList(popSortValue.value);
	}, 500);
})
</script>