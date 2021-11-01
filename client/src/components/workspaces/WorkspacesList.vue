<template>
	<div>
			<n-menu :options="menuOptions" />
	</div>
</template>

<script lang="ts" setup>
import { WorkspaceItem } from '@/interfaces';
import { h } from '@vue/runtime-core';
import { MenuGroupOption, MenuOption } from 'naive-ui';
import { RouterLink } from 'vue-router';

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

const menuOptions: Array<MenuOption | MenuGroupOption> = [
	{
		type: 'group',
		label: 'Ваши рабочие пространства',
		key: 'group',
		children: workspaces.map(item => ({
			label: () => 
				h(RouterLink, { to: `/workspaces/${item.id}` }, { default: () => item.name }),
				key: `workspace-${item.id}`
			})
		)
	}
] 
</script>