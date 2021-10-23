<template>
	<n-h4><n-text type="primary">Изменение email</n-text></n-h4>
	<n-form ref="formRef" :rules="rules" :model="formData">
		<n-form-item ref="emailInputRef" first :show-label="false" path="email.value">
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
		<n-button-group style="margin-right: 1rem">
			<n-button
				attr-type="submit"
				ghost
				@click="submitForm"
				:loading="isSubmitLoading"
				:disabled="isSubmitLoading || (isSubmitDisabled != 0)"
				v-if="isSubmitButtonShown && formData.email.isValid"
				type="success"
			>
				Сохранить
			</n-button>
			<n-button v-if="isSubmitDisabled" disabled type="primary" ghost>{{ isSubmitDisabled }}</n-button>
		</n-button-group>
		<n-button ghost v-if="isSubmitButtonShown" type="error" @click="undoChanges">Отменить</n-button>
	</n-form>
</template>

<script lang="ts">
import { emailRegex, holdSubmitDisabled } from '@/helpers';
import { FormRules, NForm, NFormItem, useMessage } from 'naive-ui';
import { defineComponent, reactive, ref } from 'vue'
import { useStore } from '@/store';
import { EmailEditFormData } from '@/interfaces';
import QuestionTooltip from '@/components/QuestionTooltip.vue'
import { ApiHelper } from '@/helpers/api';
import { AxiosError } from 'axios';

export default defineComponent({
	name: 'ProfileEditEmail',
	components: {
		QuestionTooltip
	},
	setup() {
		// data
		const store = useStore();
		const formRef = ref<InstanceType<typeof NForm>>();
		const defaultEmailValue: string = store.state.user.email;
		const formData = reactive<EmailEditFormData>({
			email: {
				value: defaultEmailValue,
				isValid: true,
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
				],
			},
		}
		const emailInputRef = ref<InstanceType<typeof NFormItem>>();
		const message = useMessage();
		const isSubmitButtonShown = ref<boolean>(false);
		const isSubmitLoading = ref<boolean>(false);
		const isSubmitDisabled = ref<number>(0);

		// methods
		const handleEmailInput = (value: string | null): void => {
			isSubmitButtonShown.value = !(value === defaultEmailValue);
		}
		const undoChanges = (): void => {
			formData.email.value = defaultEmailValue;
			handleEmailInput(formData.email.value);
			emailInputRef.value?.restoreValidation();
		}
		const submitForm = (): void => {
			isSubmitLoading.value = true;
      formRef.value?.validate((errors) => {
        if (!errors) {
					ApiHelper.userGetByEmail({ email: formData.email.value })
						.then((response) => {
							if (response.data.statusCode == 200) {
								message.warning('Данный email занят другим пользователем');
							}
						})
						.catch((error: AxiosError) => {
							if (error.response?.status === 404) {
								store.dispatch('updateEmail', formData.email.value)
								.then(() => {
									message.success('Вы успешно изменили email');
								})
								.catch((error) => {
									message.error(error.message);
								})
							} else {
								console.log(error.response);
								message.warning('Проверка адреса не завершена');
							}
						})
						.finally(() => {
							isSubmitLoading.value = false;
							holdSubmitDisabled(isSubmitDisabled);
						});
        } else {
          message.error('Данные не являются корректными');
					isSubmitLoading.value = false;
					holdSubmitDisabled(isSubmitDisabled);
        }
      });
    };
		return {
			store,
			formRef,
			formData,
			defaultEmailValue,
			rules,
			isSubmitButtonShown,
			emailInputRef,
			isSubmitLoading,
			isSubmitDisabled,
			handleEmailInput,
			undoChanges,
			submitForm
		}
	},
})
</script>
