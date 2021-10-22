<template>
	<n-h4><n-text type="primary">Изменение email</n-text></n-h4>
	<n-form>
		<n-form-item :show-label="false" path="email.value">
			<n-auto-complete ref="emailInput" @update:value="handleEmailInput" placeholder="" :default-value="store.state.user.email" />
		</n-form-item>
		<n-button v-if="submitButtonShown">Сохранить изменения</n-button>
	</n-form>
	<n-h4><n-text type="primary">Изменение пароля</n-text></n-h4>
	<n-form>
		<n-form-item label="Текущий пароль" path="password.value">
			<n-input
				type="password"
				show-password-on="click"
				placeholder=""
			/>
		</n-form-item>
		<n-form-item label="Новый пароль" path="password.value">
			<n-input type="password" show-password-on="click" placeholder="" />
		</n-form-item>
		<n-form-item label="Повторите новый пароль" path="password.value">
			<n-input type="password" show-password-on="click" placeholder="" />
		</n-form-item>
		<n-button>Изменить пароль</n-button>
	</n-form>
</template>

<script lang="ts">
import { useStore } from '@/store';
import { NAutoComplete } from 'naive-ui';
import { defineComponent, ref } from 'vue'

export default defineComponent({
	name: 'ProfileEmail',
	setup() {
		const store = useStore();
		const submitButtonShown = ref<boolean>(false);
		const emailInput = ref<InstanceType<typeof NAutoComplete>>(); 
		return {
			store,
			submitButtonShown,
			emailInput,
			handleEmailInput(value: string | null): void {
				submitButtonShown.value = !(value === emailInput.value?.defaultValue);
			}
		}
	}
})
</script>
