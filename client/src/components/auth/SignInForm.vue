<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item first label="Email" path="email.value">
      <n-auto-complete
        :options="options"
        placeholder=""
        v-model:value="formData.email.value"/>
    </n-form-item>
    <n-form-item first label="Пароль" path="password.value">
      <n-input
        type="password"
        show-password-on="click"
        placeholder=""
        v-model:value="formData.password.value"/>
    </n-form-item>
    <n-form-item label-placement="left" label="Запомнить меня" path="">
      <n-checkbox v-model:checked="formData.remembered.value" />
    </n-form-item>
    
    <n-form-item>
			<delayed-button
				ref="submitButtonRef"
				attr-type="submit"
				type="primary"
				ghost
				:loading="submitLoading"
				:disabled="!formData.email.isValid || !formData.password.value || submitLoading"
				@click="submitForm">
				Войти
			</delayed-button>
    </n-form-item>
  </n-form>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { NForm, useMessage, FormRules } from 'naive-ui'
import { useRouter } from 'vue-router'
import { useStore } from '@/store'
import { emailRegex, externalOptions } from '@/helpers'
import { SignInFormData } from '@/interfaces'
import DelayedButton from '@/components/DelayedButton.vue'
import { UserState } from '@/interfaces/store'

const formData = reactive<SignInFormData>({
	email: {
		value: '',
		isValid: false,
	},
	password: {
		value: '',
		isValid: false,
	},
	remembered: {
		value: false,
	},
});
const rules: FormRules = {
	email: {
		value: [
			{
				asyncValidator: (rule, value) => {
					return new Promise<void>((resolve, reject) => {
						if (emailRegex.test(value)) {
							formData.email.isValid = true;
							resolve();
						} else {
							formData.email.isValid = false;
							if (formData.email.value === '') {
								resolve()
							} else {
								reject(new Error('Введите корректный email'));
							}
						}
					});
				},
				trigger: ['blur', 'input'],
			},
		],
	},
};
const formRef = ref<InstanceType<typeof NForm>>();
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const message = useMessage();
const submitLoading = ref<boolean>(false);
const store = useStore();
const router = useRouter();
const options = computed(() => externalOptions(formData.email.value));

function submitForm(): void {
	submitLoading.value = true;
	formRef.value?.validate(async (errors): Promise<void> => {
		if (!errors) {
			try {
				const userState: UserState = await store.dispatch('user/login', formData);
				const signinBC = new BroadcastChannel('signin');
				signinBC.postMessage(userState);
				signinBC.close();
				message.success('Вы успешно вошли!');
				router.push('/profile');
			} catch (error) {
				if(error instanceof Error) {
					message.error(error.message);
				}
			} finally {
				submitLoading.value = false;
				submitButtonRef.value?.holdDisabled();
			}
		} else {
			message.error('Данные не являются корректными');
			submitLoading.value = false;
			submitButtonRef.value?.holdDisabled();
		}
	});
}
</script>

