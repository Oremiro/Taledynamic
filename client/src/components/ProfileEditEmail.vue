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
		<n-button ghost v-if="submitButtonShown && formData.email.isValid" type="success" style="margin-right: 1rem">Сохранить</n-button>
		<n-button ghost v-if="submitButtonShown" type="error" @click="undoChanges">Отменить</n-button>
	</n-form>
</template>

<script lang="ts">
import { emailRegex } from '@/helpers';
import { FormRules, NForm, NFormItem } from 'naive-ui';
import { defineComponent, reactive, ref } from 'vue'
import { useStore } from '@/store';
import { EmailEditFormData } from '@/interfaces';
import QuestionTooltip from '@/components/QuestionTooltip.vue'

export default defineComponent({
	name: 'ProfileEditEmail',
	components: {
		QuestionTooltip
	},
	setup() {
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
		const submitButtonShown = ref<boolean>(false);
		const emailInputRef = ref<InstanceType<typeof NFormItem>>();
		const handleEmailInput = (value: string | null): void => {
				submitButtonShown.value = !(value === defaultEmailValue);
		}
		const undoChanges = (): void => {
			formData.email.value = defaultEmailValue;
			handleEmailInput(formData.email.value);
			emailInputRef.value?.restoreValidation();
		}
		return {
			store,
			formRef,
			formData,
			defaultEmailValue,
			rules,
			submitButtonShown,
			emailInputRef,
			handleEmailInput,
			undoChanges
		}
	},
})
</script>
