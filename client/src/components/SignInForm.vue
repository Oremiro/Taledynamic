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
			<n-button-group>
				<n-button
					attr-type="submit"
					type="primary"
					ghost
					:loading="submitLoading"
					:disabled="!formData.email.isValid || !formData.password.value || submitLoading || (submitDisabled != 0)"
					@click="submitForm">Войти</n-button>
				<n-button v-if="submitDisabled" disabled type="primary" ghost>{{ submitDisabled }}</n-button>
			</n-button-group>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>
</style>

<script lang="ts">
import { computed, defineComponent, reactive, ref } from 'vue'
import { NForm, useMessage, FormRules } from "naive-ui"
import { emailRegex, externalOptions, holdSubmitDisabled } from "@/helpers"
import { SignInFormData } from '@/interfaces'
import { useStore } from '@/store'
import { useRouter } from 'vue-router'

export default defineComponent({
  
  name: "SignInForm",
  setup() {
		// data
    const formData = reactive<SignInFormData>({
      email: {
        value: "",
        isValid: false,
      },
      password: {
        value: "",
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
                  if (formData.email.value === "") {
                    resolve()
                  } else {
                    reject(new Error("Введите корректный email"));
                  }
                }
              });
            },
            trigger: ["blur", "input"],
          },
        ],
      },
    };
    const formRef = ref<InstanceType<typeof NForm>>();
    const message = useMessage();
    const submitLoading = ref<boolean>(false);
		const submitDisabled = ref<number>(0);
		const store = useStore();
		const router = useRouter();

		// methods
    const submitForm = (): void => {
			submitLoading.value = true;
      formRef.value?.validate((errors) => {
        if (!errors) {		
					store.dispatch('login', formData)
					.then(() => {
						message.success('Вы успешно вошли!');
						router.push('/profile');
					})
					.catch((error) => {
						message.error(error.message);
					})
					.finally(() => {
						submitLoading.value = false;
						holdSubmitDisabled(submitDisabled);	
					});
        } else {
          message.error('Данные не являются корректными');
					submitLoading.value = false;
					holdSubmitDisabled(submitDisabled);
        }
      });
    };
    return {
			formRef,
      formData,
      rules,
      submitLoading,
			submitDisabled,
      message,
      submitForm,
      options: computed(() => externalOptions(formData.email.value))
    }
  }
})
</script>

