<template>
	<n-h4><n-text type="primary">Синхронизация с ботом</n-text></n-h4>
	<n-text code>Тут что-то будет</n-text>
	<n-h4><n-text type="primary">Удаление аккаунта</n-text></n-h4>
	<delayed-button 
		ref="submitButtonRef" 
		:type="buttonType"
		ghost
		@click="showDeletionConfirmation"
		style="margin-right: 1rem">{{ buttonText }}</delayed-button>
	<n-button v-show="isDeletionConfirmationShown" @click="cancelDeletion">Нет, я передумал</n-button>
	<n-collapse-transition :show="isDeletionConfirmationShown">
		<div style="margin-top: 1rem">Вы уверены, что хотите удалить аккаунт?</div>
	</n-collapse-transition>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import { useRouter } from "vue-router";
import { useMessage } from "naive-ui";
import { useStore } from "@/store";
import DelayedButton from '@/components/DelayedButton.vue'

export default defineComponent({
	name: 'ProfileSettingsSection',
	components: {
		DelayedButton
	},
	setup() {
		const router = useRouter();
		const store = useStore();
		const message = useMessage();
		const isDeletionConfirmationShown = ref<boolean>(false);
		const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
		const buttonText = ref<string>('Удалить');
		const buttonType = ref<string>('default');


		const deleteUser = async (): Promise<void> => {
			try {
				await store.dispatch('user/delete');
				router.push({ name: 'Auth'});
				message.info('Вы успешно удалили аккаунт');
			} catch (error) {
				if (error instanceof Error) {
					message.error(error.message); 
				}
			}
		}

		const showDeletionConfirmation = async (): Promise<void> => {
			if (isDeletionConfirmationShown.value) {
				await deleteUser();
			} else {
				isDeletionConfirmationShown.value = true;
				buttonText.value = 'Да';
				buttonType.value = 'error';
				submitButtonRef.value?.holdDisabled();
			}
		}

		const cancelDeletion = () => {
			isDeletionConfirmationShown.value = false;
			buttonText.value = 'Удалить';
			buttonType.value = 'default';
			submitButtonRef.value?.cancelHolding();
		}

		return { 
			isDeletionConfirmationShown,
			submitButtonRef,
			buttonText,
			buttonType,
			showDeletionConfirmation,
			cancelDeletion
		};
	},
});
</script>
