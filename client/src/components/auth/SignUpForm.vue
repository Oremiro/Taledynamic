<template>
	<n-form ref="formRef" :rules="rules" :model="formData">
		<n-form-item first label="Email" path="email.value">
			<n-auto-complete 
			:options="options" 
			v-model:value="formData.email.value" 
			#default="{ handleInput, handleBlur, handleFocus, value }">
				<n-input placeholder="" @input="handleInput" @focus="handleFocus" @blur="handleBlur" :value="value">
					<template v-if="!formData.email.isValid" #prefix>
						<question-tooltip text="Email может содержать только буквы латинского алфавита, цифры, точку, подчеркивание и минус. Почтовый домен должен быть корректным."/>
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
					<question-tooltip text="Пароль должен содержать минимум 8 символов, заглавную букву, строчную букву, цифру и специальный символ."/>
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

<script lang="ts">
import { computed, defineComponent, reactive, ref } from 'vue'
import { useMessage, NForm, FormRules, NFormItem } from "naive-ui";
import { useRouter } from 'vue-router';
import { AxiosError } from 'axios';
import { useStore } from '@/store';
import { emailRegex, passwordRegex, externalOptions } from "@/helpers";
import { UserApi } from '@/helpers/api/user';
import { SignUpFormData } from '@/interfaces'
import QuestionTooltip from "@/components/QuestionTooltip.vue"
import DelayedButton from "@/components/DelayedButton.vue"

export default defineComponent({
	name: 'AuthSignUpForm',
	components: {
		QuestionTooltip, DelayedButton
	},
	setup() {
		// data
		const formData = reactive<SignUpFormData>({
			email: {
				value: "",
				isValid: false,
			},
			password: {
				value: "",
				isValid: false,
			},
			confirmedPassword: {
				value: "",
				isValid: false,
			},
		});
		const rules: FormRules = {
			email: {
				value: [
					{
						required: true,
						message: 'Пожалуйста, введите email',
						trigger: 'blur',
					},
					{
						asyncValidator: (rule, value) => {
							return new Promise<void>((resolve, reject) => {
								formData.email.isValid = false;
								if (!emailRegex.test(value)) {
									reject(new Error('Введите корректный email'));
								} else {
									formData.email.isValid = true;
									resolve();
								}
							});
						},
						trigger: ['blur', 'input'],
					},
					{
						asyncValidator: (rule, value) => 
							new Promise<void>((resolve, reject) => {
								formData.email.isValid = false;
								UserApi.isEmailUsed({ email: value })
								.then((response) => {
									if(response.data.isEmailUsed) {
										reject(new Error('Данный email занят другим пользователем'));
									} else {
										formData.email.isValid = true;
										resolve();
									}
								})
								.catch((error: AxiosError) => {
									reject(new Error(error.message));
								});
							}),
						trigger: 'blur'
					}
				],
			},
			password: {
				value: [
					{
						required: true,
						message: "Пожалуйста, введите пароль",
						trigger: ['blur']
					},
					{
						asyncValidator: (rule, value) => {
							return new Promise<void>((resolve, reject) => {
								if (!passwordRegex.test(value)) {
									formData.password.isValid = false;
									reject(
										new Error("Введите корректный сложный пароль")
									);
								} else {
									formData.password.isValid = true;
									resolve();
								}
							});
						},
						trigger: ["blur", "input"],
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
						asyncValidator: (rule, value) => {
							return new Promise<void>((resolve, reject) => {
								if (value !== formData.password.value) {
									formData.confirmedPassword.isValid = false;
									reject(new Error('Пароли не совпадают'));
								} else {
									formData.confirmedPassword.isValid = true;
									resolve();
								}
							});
						},
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

		// methods
		const handlePasswordInput = (): void => {
			if (formData.confirmedPassword.value != '') {
				confirmedPasswordRef.value?.validate({ trigger: 'password-input'}).catch(() => true);
			}
		}
    const submitForm = (): void => {
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
    };
		return {
			formData,
			rules,
			formRef,
			confirmedPasswordRef,
			submitButtonRef,
			submitLoading,
			handlePasswordInput,
			submitForm,
			options: computed(() => externalOptions(formData.email.value))
		}
	}
})
</script>
