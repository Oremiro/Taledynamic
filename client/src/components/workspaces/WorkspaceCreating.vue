<template>
<transition name="fade" mode="out-in" @enter="doAfterTransition">
	<delayed-button 
		v-if="!isWorkspaceInputShown"
		ref="workspaceCreatingButton"
		@click="isWorkspaceInputShown = true;" 
		:disabled="disabled || isWorkspaceCreatingPending" 
		:loading="isWorkspaceCreatingPending">
		{{ isWorkspaceCreatingPending ? 'Создание пространства' : 'Создать пространство' }}
	</delayed-button>
	<n-form-item v-else :show-label="false" :show-feedback="false" :rule="workspaceCreatingRule">
		<n-input-group>
			<n-input
				ref="nameInput"
				v-model:value="newWorkspaceName"
				placeholder="Название пространства"
				@keyup.enter="createWorkspace"/>
			<n-button v-if="isWorkspaceInputValid" attr-type="submit" style="padding: .6rem;" @click="createWorkspace">
				<n-icon size="1.2rem">
					<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M4.53 12.97a.75.75 0 0 0-1.06 1.06l4.5 4.5a.75.75 0 0 0 1.06 0l11-11a.75.75 0 0 0-1.06-1.06L8.5 16.94l-3.97-3.97z" fill="currentColor"></path></g></svg>
				</n-icon>
			</n-button>
			<dynamically-typed-button
				@click="clearNameInput"
				ghost
				style="padding: .6rem;"
				type="error">
				<n-icon size="1.2rem">
					<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 24 24"><g fill="none"><path d="M4.397 4.554l.073-.084a.75.75 0 0 1 .976-.073l.084.073L12 10.939l6.47-6.47a.75.75 0 1 1 1.06 1.061L13.061 12l6.47 6.47a.75.75 0 0 1 .072.976l-.073.084a.75.75 0 0 1-.976.073l-.084-.073L12 13.061l-6.47 6.47a.75.75 0 0 1-1.06-1.061L10.939 12l-6.47-6.47a.75.75 0 0 1-.072-.976l.073-.084l-.073.084z" fill="currentColor"></path></g></svg>
				</n-icon>
			</dynamically-typed-button>
		</n-input-group>
	</n-form-item>
</transition>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { FormItemRule, NInput, NInputGroup, useMessage } from 'naive-ui';
import { useStore } from '@/store';
import DynamicallyTypedButton from '@/components/DynamicallyTypedButton.vue';
import DelayedButton from '@/components/DelayedButton.vue';
import { workspaceNameValidator } from '@/helpers';

/* global defineProps */
defineProps({
	disabled: {
		type: Boolean,
		default: false
	}
})

const newWorkspaceName = ref<string>('');
const isWorkspaceInputValid = ref<boolean>(false);
const workspaceCreatingRule: FormItemRule = {
	trigger: 'input',
	asyncValidator: async (): Promise<void> => {
		isWorkspaceInputValid.value = false;
		try {
			await workspaceNameValidator(newWorkspaceName.value);
			isWorkspaceInputValid.value = true;
		} catch (error) {
			if (error instanceof Error) {
				throw error;
			}
		}
	}
}

const isWorkspaceInputShown = ref<boolean>(false);

const nameInput = ref<InstanceType<typeof NInput>>();

function doAfterTransition(): void {
	if (isWorkspaceInputShown.value) {
		nameInput.value?.focus();
	}
}

function clearNameInput(): void {
	newWorkspaceName.value = '';
	isWorkspaceInputShown.value = false;
}

const store = useStore();
const message = useMessage();
const creatingButtonHolding = ref<boolean>(false);
const isWorkspaceCreatingPending = ref<boolean>(false);

async function createWorkspace(): Promise<void> {
	isWorkspaceInputShown.value = false;
	if (!isWorkspaceInputValid.value) {
		return;
	}
	isWorkspaceCreatingPending.value = true;
	try {
		await store.dispatch('workspaces/create', { name: newWorkspaceName.value });
		message.success(`Рабочее пространство ${newWorkspaceName.value} создано`)
	} catch (error) {
		if (error instanceof Error) {
			creatingButtonHolding.value = true;
			message.error(error.message);
		}	
	} finally {
		isWorkspaceCreatingPending.value = false;
		isWorkspaceInputValid.value = false;
		newWorkspaceName.value = '';
	}
}

const workspaceCreatingButton = ref<InstanceType<typeof DelayedButton>>();

watch(workspaceCreatingButton, (value): void => {
	if (creatingButtonHolding.value) {
		value?.holdDisabled();
		creatingButtonHolding.value = false;
	}
})
</script>