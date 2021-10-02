<template>
  <n-form>
    <n-form-item label="Email" path="">
      <n-input placeholder="" v-model:value="formData.email" />
    </n-form-item>
    <n-form-item label="Пароль" path="">
      <n-input type="password" show-password-on="click" placeholder="" v-model:value="formData.password" />
    </n-form-item>
    <n-form-item label-placement="left" label="Запомнить меня" path="">
      <n-checkbox v-model:checked="formData.remembered" />
    </n-form-item>
    <n-form-item>
      <n-button :disabled="v$.formData.$invalid">Войти</n-button>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>

</style>

<script>
import axios from 'axios'
import useVuelidate from '@vuelidate/core'
import { required, email, helpers } from '@vuelidate/validators'

const requiredValidator = helpers.withMessage('Поле не должно быть пустым', required);
const emailValidator = helpers.withMessage('Неверный Email', email);

const passwordValidator = helpers.withMessage('', (value) => {
  const containsUppercase = /[A-Z]/.test(value);
  const containsLowercase = /[a-z]/.test(value);
  const containsDigit = /\d/.test(value);
  const containsSpecial = /[#?!@$%^&*_(),.+-]/.test(value);
  return containsUppercase && containsLowercase && containsDigit && containsSpecial;
})

export default {
    name: 'SignInForm',
    setup() {
      return { v$: useVuelidate() }
    },
    data() {
      return {
        formData: {
          email: '',
          password: '',
          remembered: false
        }
      }
    },
    validations() {
      return {
        formData: {
          email: { requiredValidator, emailValidator },
          password: { requiredValidator, passwordValidator},
        }
      }
    },
    methods: {
      submitForm() {
        axios.post('', this.formData)
          .then((response) => { console.log(response) })
          .catch((error) => { console.log(error) });
      }
    }
}
</script>