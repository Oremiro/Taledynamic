<template>
	<div>
		<div style="padding: 0rem 1rem .5rem 1rem; display: flex; align-items: center; justify-content: space-between;">
			<n-text depth="3">Ваши рабочие пространства</n-text>
			<n-popselect trigger="click" :options="popOptions" v-model:value="popValue" @update:value="updateHandler">
				<n-button text>
					<n-icon size="1.2rem">
						<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M17 19.44l3.22-3.22a.75.75 0 1 1 1.06 1.06l-4.5 4.5a.75.75 0 0 1-1.06 0l-4.5-4.5a.75.75 0 1 1 1.06-1.06l3.22 3.22V2.75a.75.75 0 0 1 1.5 0v16.69zM7.072 2a.75.75 0 0 1 .697.487l3.184 8.5a.75.75 0 0 1-1.405.526L8.794 9.5H5.238l-.79 2.023a.75.75 0 0 1-1.397-.546l3.317-8.5A.75.75 0 0 1 7.072 2zm-.02 2.85L5.823 8h2.41l-1.18-3.15zM4.75 13a.75.75 0 1 0 0 1.5h3.388l-4.021 6.349a.75.75 0 0 0 .633 1.15H9.5a.75.75 0 1 0 0-1.5H6.113l4.021-6.348A.75.75 0 0 0 9.5 13H4.75z" fill="currentColor"></path></g></svg>
					</n-icon>
				</n-button>
			</n-popselect>
		</div>
		<n-menu :options="menuOptions" :indent="30" />
	</div>
</template>

<script lang="ts" setup>
import { WorkspaceItem } from '@/interfaces';
import { ref, h } from 'vue';
import { MenuGroupOption, MenuOption, SelectGroupOption, SelectOption } from 'naive-ui';
import WorkspacesListItem from '@/components/workspaces/WorkspacesListItem.vue';
import { NPopselect } from 'naive-ui';

const workspaces: WorkspaceItem[] = [
	{
		id: 0,
		name: 'Университет'
	},
	{
		id: 1,
		name: 'Работа'
	},
]

const popValue = ref<string | null>('sortAscendingByDate');

const popOptions: Array<SelectOption | SelectGroupOption> = [
	{
		type: 'group',
		label: 'По дате',
		key: 'sortByDate',
		children: [
			{
				label: 'Сначала новые',
				value: 'sortAscendingByDate'
			},
			{
				label: 'Сначала старые',
				value: 'sortDescendingByDate'
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

const menuOptions: Array<MenuOption | MenuGroupOption> = workspaces.map(item => ({
	label: () => 
		h(WorkspacesListItem, { to: `/workspaces/${item.id}` }, { default: () => item.name }),
		key: `workspace-${item.id}`
	})
)

const updateHandler = (value: string) => {
	console.log(value);	
}
</script>