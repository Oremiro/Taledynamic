<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item first label="Email" path="email.value">
      <n-auto-complete :options="options" placeholder="" minlength="8" v-model:value="formData.email.value"/>
    </n-form-item>
    <n-form-item first label="Пароль" path="password.value">
      <n-input type="password" show-password-on="click" placeholder="" v-model:value="formData.password.value" />
    </n-form-item>
    <n-form-item label-placement="left" label="Запомнить меня" path="">
      <n-checkbox v-model:checked="formData.remembered.value" />
    </n-form-item>
    <n-form-item>
      <n-button type="primary" ghost :loading="submitLoading" :disabled="!formData.email.value || !formData.password.value" @click="handleValidateClick">Войти</n-button>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>

</style>

<script>
import { useMessage } from 'naive-ui'
import { emailRegex, externalOptions } from '@/variables/auth-vars.js'


export default {
    name: 'SignInForm',
    data() {
      return {
        submitLoading: false,
        message: useMessage(),
        formData: {
          email: {
            value: '',
            isValid: false
          },
          password: {
            value: '',
            isValid: false
          },
          remembered: {
            value: false
          }
        },
        rules: {
          email: { 
            value: [
              {
                pattern: emailRegex,
                message: 'Введите корректный email',
                trigger: ['input', 'blur']
              }
            ],
          }
        }
      }
    },
    computed: {
      options() {
        return externalOptions(this.formData.email.value);
      }
    },
    methods: {
      handleValidateClick() {
        this.submitLoading = true;
        // this.message.loading('aaaaa');
        this.$refs.formRef.validate((errors) => {
          if (!errors) {
            this.message.success('Valid')
          } else {
            this.message.error('Invalid')
          }
          this.submitLoading = false;
        })
      }
    }
}
</script>