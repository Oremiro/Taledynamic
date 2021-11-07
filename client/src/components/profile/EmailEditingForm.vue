<template>
	<n-form ref="formRef" :rules="rules" :model="formData">
		<n-form-item ref="emailInputRef" first label="Email" path="email.value">
			<n-auto-complete 
			v-model:value="formData.email.value" 
			@update:value="handleEmailInput"
			#default="{ handleInput, handleBlur, handleFocus, value }"
			>
				<n-input placeholder="" @input="handleInput" @focus="handleFocus" @blur="handleBlur" :value="value">
					<template v-if="!formData.email.isValid" #prefix>
						<question-tooltip text="Email может содержать только буквы латинского алфавита, цифры, точку, подчеркивание и минус. Почтовый домен должен быть корректным."/>
					</template>
				</n-input>
			</n-auto-complete>
		</n-form-item>
		<n-collapse-transition :show="isSubmitButtonShown">
			<n-form-item label="Текущий пароль" path="currentPassword.value">
				<n-input
					type="password"
					show-password-on="click"
					placeholder=""
					v-model:value="formData.currentPassword.value"
				/>
			</n-form-item>
			<delayed-button 
				ref="submitButtonRef" 
				attr-type="submit" 
				type="success" 
				ghost 
				style="margin-right: 1rem" 
				@click="submitForm" 
				:disabled="isSubmitButtonLoading"
				:loading="isSubmitButtonLoading"
				v-show="formData.email.isValid && formData.currentPassword.value">
				Сохранить
			</delayed-button>
			<n-button ghost type="error" @click="undoChanges">Отменить</n-button>
		</n-collapse-transition>
	</n-form>
</template>

<script setup lang="ts">
import { emailRegex } from '@/helpers';
import { FormRules, NForm, NFormItem, useMessage } from 'naive-ui';
import { reactive, ref } from 'vue'
import { useStore } from '@/store';
import { EmailEditFormData } from '@/interfaces';
import QuestionTooltip from '@/components/QuestionTooltip.vue'
import DelayedButton from '@/components/DelayedButton.vue'
import { UserApi } from '@/helpers/api/user';
import { AxiosError } from 'axios';

const store = useStore();
const formRef = ref<InstanceType<typeof NForm>>();
const defaultEmailValue = ref<string>(store.getters['user/email']);
const formData = reactive<EmailEditFormData>({
	email: {
		value: defaultEmailValue.value,
		isValid: true,
	},
	currentPassword: {
		value: '',
		isValid: false
	}
});
const rules: FormRules = {
	email: {
		value: [
			{
				required: true,
				message: 'Пожалуйста, введите email',
				trigger: ['blur', 'input']
			},
			{
				asyncValidator: (rule, value) => 
					new Promise<void>((resolve, reject) => {
						formData.email.isValid = false;
						if (!emailRegex.test(value)) {
							reject(new Error('Введите корректный email'));
						} else {
							formData.email.isValid = true;
							resolve();
						}
					}),
				trigger: ['blur', 'input'],
			},
			{
				asyncValidator: (rule, value) => 
					new Promise<void>((resolve, reject) => {
						if (value === defaultEmailValue.value) {
							resolve()
						} else {
							UserApi.isEmailUsed({ email: value })
							.then((response) => {
								if(response.data.isEmailUsed) {
									reject(new Error('Данный email занят другим пользователем'));
								} else {
									resolve();
								}
							})
							.catch((error: AxiosError) => {
								reject(new Error(error.message));
							});
						}
					}),
				trigger: 'blur'
			}
		],
	},
	currentPassword: {
		value: [
			{
				required: true,
				message: 'Пожалуйста, введите текущий пароль',
				trigger: 'blur'
			}
		]
	},
}
const emailInputRef = ref<InstanceType<typeof NFormItem>>();
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const message = useMessage();
const isSubmitButtonShown = ref<boolean>(false);
const isSubmitButtonLoading = ref<boolean>(false);


function handleEmailInput(value: string | null): void {
	isSubmitButtonShown.value = !(value === defaultEmailValue.value);
}
function undoChanges(): void {
	formData.email.value = defaultEmailValue.value;
	formData.currentPassword.value = '';
	isSubmitButtonShown.value = false;
	emailInputRef.value?.restoreValidation();
}
function saveChanges(): void {
	formData.currentPassword.value = '';
	defaultEmailValue.value = store.getters['user/email'];
	isSubmitButtonShown.value = false;
}
function submitForm(): void {
	isSubmitButtonLoading.value = true;
	formRef.value?.validate(async (errors) => {
		if (!errors) {
			try {
				await store.dispatch('user/updateEmail', { 
					currentPassword: formData.currentPassword.value, 
					newEmail: formData.email.value 
				});
				saveChanges();
				message.success('Вы успешно изменили email');
			} catch (error) {
				if (error instanceof Error) {
					message.error(error.message);
				}
			}
		} else {
			message.error('Данные не являются корректными');
		}
		isSubmitButtonLoading.value = false;
		submitButtonRef.value?.holdDisabled();
	});
}
</script>
