<template>
	<n-h4><n-text type="primary">Изменение пароля</n-text></n-h4>
	<n-form ref="formRef" :model="formData" :rules="rules">
		<n-form-item first label="Новый пароль" path="newPassword.value">
			<n-input 
				type="password" 
				show-password-on="click" 
				placeholder="" 
				v-model:value="formData.newPassword.value"
				@input="handlePasswordInput"
			>
				<template v-if="!formData.newPassword.isValid" #prefix>
					<question-tooltip text="Пароль должен содержать минимум 8 символов, заглавную букву, строчную букву, цифру и специальный символ."/>
				</template>
			</n-input>
		</n-form-item>
		<n-form-item first ref="confirmedPasswordRef" label="Повторите новый пароль" path="confirmedPassword.value">
			<n-input type="password" show-password-on="click" placeholder="" v-model:value="formData.confirmedPassword.value" />
		</n-form-item>
		<n-collapse-transition :collapsed="formData.newPassword.value !== '' || formData.confirmedPassword.value !== ''">
			<n-form-item first label="Текущий пароль" path="currentPassword.value">
				<n-input
					type="password"
					show-password-on="click"
					placeholder=""
					v-model:value="formData.currentPassword.value"
				/>
			</n-form-item>
		</n-collapse-transition>
		<n-button-group 
			style="margin-right: 1rem"
			v-if="formData.currentPassword.value && formData.newPassword.isValid && formData.confirmedPassword.isValid"
		>
			<n-button
				attr-type="submit"
				ghost
				type="success"
				@click="submitForm"
				:loading="isSubmitButtonLoading"
				:disabled="isSubmitButtonLoading || (isSubmitButtonDisabled != 0)"
			>
				Сохранить
			</n-button>
			<n-button v-if="isSubmitButtonDisabled" disabled type="primary" ghost>{{ isSubmitButtonDisabled }}</n-button>
		</n-button-group>
		<n-button 
			ghost 
			type="error" 
			@click="undoChanges"
			v-if="formData.currentPassword.value || formData.newPassword.value || formData.confirmedPassword.value"
		>
			Отменить
		</n-button>
	</n-form>
</template>

<script lang="ts">
import { holdSubmitDisabled, passwordRegex } from '@/helpers';
import { PasswordEditFormData } from '@/interfaces';
import { FormRules, NForm, NFormItem, useMessage } from 'naive-ui';
import { defineComponent, reactive, ref } from 'vue'
import QuestionTooltip from '@/components/QuestionTooltip.vue'
import { useStore } from '@/store';

export default defineComponent({
	name: 'ProfileEditPassword',
	components: {
		QuestionTooltip
	},
	setup() {
		// data
		const formRef = ref<InstanceType<typeof NForm>>();
		const formData = reactive<PasswordEditFormData>({
			currentPassword: {
				value: '',
				isValid: false
			},
			newPassword: {
				value: '',
				isValid: false,
			},
			confirmedPassword: {
				value: '',
				isValid: false,
			},
		});
		const rules: FormRules = {
			currentPassword: {
				value: [
					{
						required: true,
						message: 'Пожалуйста, введите текущий пароль',
						trigger: ['blur']
					}
				]
			},
			newPassword: {
				value: [
					{
						required: true,
						message: 'Пожалуйста, введите новый пароль',
						trigger: 'blur'
					},
					{
						asyncValidator: (rule, value) => {
							return new Promise<void>((resolve, reject) => {
								if (!passwordRegex.test(value)) {
									formData.newPassword.isValid = false;
									reject(
										new Error("Введите корректный сложный пароль")
									);
								} else {
									formData.newPassword.isValid = true;
									resolve();
								}
							});
						},
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
						asyncValidator: (rule, value) => {
							return new Promise<void>((resolve, reject) => {
								if (value !== formData.newPassword.value) {
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
		const store = useStore();
		const message = useMessage();
		const confirmedPasswordRef = ref<InstanceType<typeof NFormItem>>();

		const isSubmitButtonLoading = ref<boolean>(false);
		const isSubmitButtonDisabled = ref<number>(0);

		// methods
		const handlePasswordInput = (): void => {
			if (formData.confirmedPassword.value != '') {
				confirmedPasswordRef.value?.validate({ trigger: 'password-input'}).catch(() => true);
			}
		}

		const undoChanges = (): void => {
			formData.currentPassword.value = '';
			formData.newPassword.value = '';
			formData.confirmedPassword.value = '';
			formRef.value?.restoreValidation();
		}

		const submitForm = (): void => {
			isSubmitButtonLoading.value = true;
      formRef.value?.validate(async (errors) => {
        if (!errors) {
					try {
						await store.dispatch('updatePassword', {
							currentPassword: formData.currentPassword.value, 
							newPassword: formData.newPassword.value, 
							confirmedNewPassword: formData.confirmedPassword.value 
						})
						undoChanges();
						message.success('Вы успешно изменили пароль');
					} catch (error) {
						if (error instanceof Error) {
							message.error(error.message);
						}
					} finally {
						isSubmitButtonLoading.value = false;
						holdSubmitDisabled(isSubmitButtonDisabled);
					}
        } else {
          message.error('Данные не являются корректными');
					isSubmitButtonLoading.value = false;
					holdSubmitDisabled(isSubmitButtonDisabled);
        }
      });
    };

		return {
			formRef,
			formData,
			rules,
			confirmedPasswordRef,
			isSubmitButtonLoading,
			isSubmitButtonDisabled,
			handlePasswordInput,
			undoChanges,
			submitForm
		}
	},
})
</script>