<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item first label="Email" path="email.value">
      <n-auto-complete :options="options" placeholder="" minlength="8" v-model:value="formData.email.value"/>
    </n-form-item>
    <n-form-item first label="Пароль" path="password.value">
      <n-input type="password" show-password-on="click" placeholder="" v-model:value="formData.password.value" />
    </n-form-item>
    <n-form-item ref="pwdRef" first label="Повторите пароль" path="repeatedPassword.value">
      <n-input type="password" show-password-on="click" placeholder="" v-model:value="formData.repeatedPassword.value" />
    </n-form-item>
    
    <n-form-item>
      <n-button type="primary" ghost :loading="submitLoading" :disabled="!formData.email.isValid  || !formData.password.isValid " @click="handleValidateClick">Зарегистрироваться</n-button>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>

</style>

<script>
import { useMessage } from 'naive-ui'
import { emailRegex, passwordRegex, externalOptions } from '@/variables/auth-vars.js'

export default {
    name: 'SignUpForm',
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
          repeatedPassword: {
            value: '',
            isValid: false
          }
        },
        rules: {
          email: { 
            value: [
              {
                required: true,
                message: 'Пожалуйста, введите email',
                trigger: 'blur'
              },
              {
                asyncValidator: (rule, value) => {
                  return new Promise((resolve, reject) => {
                    this.formData.email.isValid = false
                    if (!emailRegex.test(value)) {
                      reject(new Error('Введите корректный email'));
                    } else {
                      this.formData.email.isValid = true
                      resolve();
                    }
                  });
                },
                trigger: ['blur', 'input']
              },
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
                asyncValidator: (rule, value) => {
                  return new Promise((resolve, reject) => {
                    if (!passwordRegex.test(value)) {
                      this.formData.password.isValid = false
                      reject(
                        new Error('Пароль должен содержать минимум 8 символов, заглавную букву, строчную букву, цифру и специальный символ')
                      );
                    } else if (this.formData.repeatedPassword.value !== '' && value !== this.formData.repeatedPassword.value) {
                      this.formData.password.isValid = false
                      reject(
                        new Error('Пароли не совпадают')
                      );
                    } else {
                      this.formData.password.isValid = true
                      resolve();
                    }
                  });
                },
                trigger: ['blur', 'input']
              }
            ]
          },
          repeatedPassword: {
            value: [
              {
                required: true,
                message: 'Пожалуйста, повторите пароль',
                trigger: 'blur'
              },
              {
                asyncValidator: (rule, value) => {
                  return new Promise((resolve, reject) => {
                    if (value !== this.formData.password.value) {
                      this.formData.repeatedPassword.isValid = false
                      reject(
                        new Error('Пароли не совпадают')
                      );
                    } else {
                      this.formData.repeatedPassword.isValid = true
                      resolve();
                    }
                  });
                },
                trigger: ['blur', 'input']
              }
            ]
          }
        }
      }
    },
    computed: {
      options() { 
        return externalOptions(this.formData.email.value)
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