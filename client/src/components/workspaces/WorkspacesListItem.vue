<template>
	<div style="display: flex; justify-content: space-between; align-items: center;">
		<n-ellipsis :tooltip="{ delay: 500, placement: 'top-end'}">
			<router-link :to="toLink">
				<slot></slot>
			</router-link>
			<template #tooltip>
				<slot></slot>
			</template>
		</n-ellipsis>
		<div style="display: flex; align-items: center;">
			<n-button text style="margin-right: .3rem;">
				<n-icon size="1.2rem">
					<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M21.03 2.97a3.578 3.578 0 0 1 0 5.06L9.062 20a2.25 2.25 0 0 1-.999.58l-5.116 1.395a.75.75 0 0 1-.92-.921l1.395-5.116a2.25 2.25 0 0 1 .58-.999L15.97 2.97a3.578 3.578 0 0 1 5.06 0zM15 6.06L5.062 16a.75.75 0 0 0-.193.333l-1.05 3.85l3.85-1.05A.75.75 0 0 0 8 18.938L17.94 9L15 6.06zm2.03-2.03l-.97.97L19 7.94l.97-.97a2.079 2.079 0 0 0-2.94-2.94z" fill="currentColor"></path></g></svg>
				</n-icon>
			</n-button>
			<n-button text>
				<n-icon size="1.2rem">
					<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 28 28"><g fill="none"><path d="M14 2.25a3.75 3.75 0 0 1 3.745 3.55l.005.2h5.5a.75.75 0 0 1 .102 1.493l-.102.007h-1.059l-1.22 15.053A3.75 3.75 0 0 1 17.233 26h-6.466a3.75 3.75 0 0 1-3.738-3.447L5.808 7.5H4.75a.75.75 0 0 1-.743-.648L4 6.75a.75.75 0 0 1 .648-.743L4.75 6h5.5A3.75 3.75 0 0 1 14 2.25zm6.687 5.25H7.313l1.211 14.932a2.25 2.25 0 0 0 2.243 2.068h6.466a2.25 2.25 0 0 0 2.243-2.068L20.686 7.5zm-8.937 3.75a.75.75 0 0 1 .743.648L12.5 12v8a.75.75 0 0 1-1.493.102L11 20v-8a.75.75 0 0 1 .75-.75zm4.5 0a.75.75 0 0 1 .743.648L17 12v8a.75.75 0 0 1-1.493.102L15.5 20v-8a.75.75 0 0 1 .75-.75zM14 3.75a2.25 2.25 0 0 0-2.245 2.096L11.75 6h4.5l-.005-.154A2.25 2.25 0 0 0 14 3.75z" fill="currentColor"></path></g></svg>
				</n-icon>
			</n-button>
		</div>
	</div>
</template>

<script lang="ts" setup>
import { RouterLink } from 'vue-router';
import { NEllipsis } from 'naive-ui'

/* global defineProps */
const props = defineProps({
	id: {
		type: Number,
		required: true
	}
});

const toLink = `/workspaces/${props.id}`;

function deleteWorkspace() {
	return
}
</script>