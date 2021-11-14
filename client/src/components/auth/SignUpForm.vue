<template>
	<n-form ref="formRef" :rules="rules" :model="formData">
		<n-form-item first label="Email" path="email.value">
			<n-auto-complete 
			:options="options" 
			v-model:value="formData.email.value" 
			#default="{ handleInput, handleBlur, handleFocus, value }">
				<n-input placeholder="" @input="handleInput" @focus="handleFocus" @blur="handleBlur" :value="value">
					<template v-if="!formData.email.isValid && !isEmailUsed" #prefix>
						<question-tooltip>
							Email может содержать только буквы латинского алфавита, цифры, точку, подчеркивание и минус. Почтовый домен должен быть корректным.
						</question-tooltip>
					</template>
				</n-input>
			</n-auto-complete>
		</n-form-item>
		<n-form-item first label="Пароль" path="password.value">
			<n-input
				type="password"
				show-password-on="click"
				placeholder=""
				v-model:value="formData.password.value"
				@input="handlePasswordInput">
				<template v-if="!formData.password.isValid" #prefix>
					<question-tooltip >
						Пароль должен содержать минимум 8 символов, заглавную букву, строчную букву, цифру и специальный символ.
					</question-tooltip>
				</template>
			</n-input>
		</n-form-item>
		<n-form-item
			ref="confirmedPasswordRef"
			first
			label="Повторите пароль"
			path="confirmedPassword.value">
			<n-input
				type="password"
				show-password-on="click"
				placeholder=""
				:disabled="!formData.password.isValid"
				v-model:value="formData.confirmedPassword.value"/>
		</n-form-item>

		<n-form-item>
			<delayed-button 
				ref="submitButtonRef" 
				attr-type="submit"
				type="primary"
				ghost
				:loading="submitLoading"
				:disabled="!formData.email.isValid || !formData.password.isValid || !formData.confirmedPassword.isValid"
				@click="submitForm">Зарегистрироваться</delayed-button>
		</n-form-item>
	</n-form>
</template>

<style lang="scss" scoped>
.n-input-question:hover {
	color: var(--icon-color-hover);
}
</style>

<script setup lang="ts">
import { computed, reactive, ref } from 'vue'
import { useMessage, NForm, FormRules, NFormItem } from 'naive-ui';
import { useRouter } from 'vue-router';
import axios from 'axios';
import { useStore } from '@/store';
import { debounce, emailRegex, passwordRegex, externalOptions } from '@/helpers';
import { UserApi } from '@/helpers/api/user';
import { SignUpFormData } from '@/interfaces'
import QuestionTooltip from '@/components/QuestionTooltip.vue'
import DelayedButton from '@/components/DelayedButton.vue'

const formData = reactive<SignUpFormData>({
	email: {
		value: '',
		isValid: false,
	},
	password: {
		value: '',
		isValid: false,
	},
	confirmedPassword: {
		value: '',
		isValid: false,
	},
});
const isEmailUsed = ref<boolean>(false);
const rules: FormRules = {
	email: {
		value: [
			{
				required: true,
				message: 'Пожалуйста, введите email',
				trigger: 'blur',
			},
			{
				asyncValidator: debounce((rule, value) => {
					if (!emailRegex.test(value)) {
						formData.email.isValid = false;
						throw new Error('Введите корректный email');
					} else {
						formData.email.isValid = true;
					}
				}, 500),
				trigger: ['blur', 'input']
			},
			{
				asyncValidator: debounce(async (rule, value) => {
					try {
						const { data } = await UserApi.isEmailUsed({ email: value });
						isEmailUsed.value = data.isEmailUsed;
						if(data.isEmailUsed) {
							throw new Error('Данный email занят другим пользователем');
						} else {
							formData.email.isValid = true;
							return;
						}
					} catch (error) {
						formData.email.isValid = false;
						if (axios.isAxiosError(error)) {
							throw new Error(error.message)
						} else {
							throw error;
						}
					}
				}, 1000, { isAwaited: true }),
				trigger: ['blur', 'input']
			}
		],
	},
	password: {
		value: [
			{
				required: true,
				message: 'Пожалуйста, введите пароль',
				trigger: 'blur'
			},
			{
				asyncValidator: debounce((rule, value) => {
					if (!passwordRegex.test(value)) {
						formData.password.isValid = false;
						throw new Error('Введите корректный сложный пароль')
					} else {
						formData.password.isValid = true;
					}
				}, 500),
				trigger: ['blur', 'input'],
			},
		],
	},
	confirmedPassword: {
		value: [
			{
				required: true,
				message: 'Пожалуйста, повторите пароль',
				trigger: 'blur',
			},
			{
				asyncValidator: debounce((rule, value) => {
					if (value !== formData.password.value) {
						formData.confirmedPassword.isValid = false;
						throw new Error('Пароли не совпадают');
					} else {
						formData.confirmedPassword.isValid = true;
					}
				}),
				trigger: ['blur', 'input', 'password-input'],
			},
		],
	},
};
const formRef = ref<InstanceType<typeof NForm>>();
const confirmedPasswordRef = ref<InstanceType<typeof NFormItem>>();
const submitButtonRef = ref<InstanceType<typeof DelayedButton>>();
const message = useMessage();
const submitLoading = ref<boolean>(false);
const store = useStore();
const router = useRouter();
const options = computed(() => externalOptions(formData.email.value));

function handlePasswordInput(): void {
	if (formData.confirmedPassword.value != '') {
		confirmedPasswordRef.value?.validate({ trigger: 'password-input'}).catch(() => true);
	}
}
function submitForm(): void {
	submitLoading.value = true;
	formRef.value?.validate(async (errors): Promise<void> => {
		if (!errors) {
			try {
				await store.dispatch('user/register', formData)
				message.success('Вы успешно зарегистрировались');
				router.push({ name: 'AuthSignIn' });
			} catch (error) {
				if (error instanceof Error) {
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
