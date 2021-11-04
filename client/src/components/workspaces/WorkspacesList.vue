<template>
	<transition name="fade" mode="out-in">
		<div v-if="isListLoading" class="workspaces-list--spin">
			<div class="spin--flex-column-center">
				<transition name="fade" mode="out-in">
					<div v-if="!isListLoadingError" class="spin--flex-column-center">
						<n-spin size="large" />
						<div style="margin-top: 1rem">
							Рабочие пространства загружаются
						</div>
					</div>
					<div v-else class="spin--flex-column-center">
						<n-text type="error" tag="div">
							<n-icon size="3.5rem">
								<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 20 20"><g fill="none"><path d="M10 2a8 8 0 1 1 0 16a8 8 0 0 1 0-16zm0 1a7 7 0 1 0 0 14a7 7 0 0 0 0-14zm0 9.5a.75.75 0 1 1 0 1.5a.75.75 0 0 1 0-1.5zM10 6a.5.5 0 0 1 .492.41l.008.09V11a.5.5 0 0 1-.992.09L9.5 11V6.5A.5.5 0 0 1 10 6z" fill="currentColor"></path></g></svg>
							</n-icon>
						</n-text>
						<n-text type="error">
							Ошибка загрузки пространств
						</n-text>
						<n-button
							@click="getWorkspaces"
							style="margin-top: 1rem">
							Попробовать снова
						</n-button>
					</div>
				</transition>
			</div>
		</div>
		<div v-else class="workspaces-list">
			<div class="workspaces-list--header">
				<n-text depth="3">Ваши рабочие пространства</n-text>
				<n-popselect trigger="click" :options="popOptions" v-model:value="popSortValue" @update:value="updateHandler">
					<n-button text :disabled="isListLoading">
						<n-icon size="1.2rem">
							<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M17.25 4l-.1.007a.75.75 0 0 0-.65.743v12.692l-3.22-3.218l-.084-.072a.75.75 0 0 0-.976 1.134l4.504 4.5l.084.072a.75.75 0 0 0 .976-.073l4.497-4.5l.072-.084a.75.75 0 0 0-.073-.977l-.084-.072a.75.75 0 0 0-.977.073L18 17.446V4.75l-.006-.102A.75.75 0 0 0 17.251 4zm-11.036.22L1.72 8.715l-.073.084a.75.75 0 0 0 .073.976l.084.073a.75.75 0 0 0 .976-.073l3.217-3.218v12.698l.008.102a.75.75 0 0 0 .743.648l.101-.007a.75.75 0 0 0 .649-.743L7.497 6.559l3.223 3.217l.084.072a.75.75 0 0 0 .975-1.134L7.275 4.22l-.085-.072a.75.75 0 0 0-.976.073z" fill="currentColor"></path></g></svg>
						</n-icon>
					</n-button>
				</n-popselect>
			</div>
			<n-scrollbar style="height: 100%;">
				<n-menu :options="menuOptions" :indent="30" style="padding: 0 .25rem;"/>
			</n-scrollbar>
			<div class="workspaces-list--footer">
				<workspace-creating-button :disabled="isListLoading" />
			</div>
		</div>
	</transition>
</template>

<style lang="scss" scoped>
.workspaces-list {
	height: 100%; 
	display: flex; 
	flex-direction: column;
}

.workspaces-list--header {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: space-between;
}
.workspaces-list--footer {
	padding: 1rem; 
	display: flex; 
	align-items: center; 
	justify-content: center;
}

.spin--flex-column-center {
	display: flex; 
	flex-direction: column; 
	align-items: center;
}
.workspaces-list--spin {
	display: flex; 
	align-items: center; 
	justify-content: center;
	height: 100%;
}
</style>

<script setup lang="ts">
import { ref, h, reactive, computed, onMounted } from 'vue';
import { MenuGroupOption, MenuOption, SelectGroupOption, SelectOption, NPopselect, NScrollbar, useMessage, NSpin } from 'naive-ui';
import { useStore } from '@/store';
import { WorkspaceItem } from '@/interfaces';
import { Workspace } from '@/interfaces/store';
import WorkspacesListItem from '@/components/workspaces/WorkspacesListItem.vue';
import WorkspaceCreatingButton from '@/components/workspaces/WorkspaceCreating.vue';

// data
const store = useStore();
const workspaces = reactive<WorkspaceItem[]>([
	{
		id: 0,
		name: 'Аббатство',
		modified: new Date('2021-11-03')
	},
	{
		id: 1,
		name: 'Работа',
		modified: new Date('2021-11-04')
	},
	{
		id: 2,
		name: 'Университет',
		modified: new Date('2020-11-04')
	}
])
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
const menuOptions = computed<Array<MenuOption | MenuGroupOption>>(
	() => store.getters['workspaces/workspaces'].map((item: Workspace) => ({
			label: () => h(WorkspacesListItem, { id: item.id }, { default: () => item.name }), 
			key: `workspace-${item.id}`
		})
	)
)

const popSortValue = ref<string>(localStorage.getItem('workspacesSort') ?? 'sortAscendingByDate');

const isListLoading = ref<boolean>(true);
const isListLoadingError = ref<boolean>(false);

// methods
const sortWorkspacesListByName = (reverse = false): void => {
	workspaces.sort((itemFirst, itemSecond) => {
		if (itemFirst.name > itemSecond.name) {
			return reverse ? -1 : 1;
		} else if (itemFirst.name < itemSecond.name) {
			return reverse ? 1 : -1;
		}
		return 0;
	})
}
const sortWorkspacesListByDate = (reverse = false): void => {
	workspaces.sort((itemFirst, itemSecond) => reverse ? 
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

const message = useMessage();
onMounted((): void => {
	sortWorkspacesList(popSortValue.value);
	setTimeout(async (): Promise<void> => {
		await getWorkspaces();
	}, 0);
})
</script>