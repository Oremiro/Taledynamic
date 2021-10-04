<template>
  <n-form ref="formRef" :rules="rules" :model="formData">
    <n-form-item first ref="emailRef" label="Email" path="email">
      <n-input placeholder="" minlength="8" v-model:value="formData.email" @input="test"/>
    </n-form-item>
    <n-form-item first label="Пароль" path="password">
      <n-input type="password" minlength="8" show-password-on="click" placeholder="" v-model:value="formData.password" />
    </n-form-item>
    <n-form-item label-placement="left" label="Запомнить меня" path="">
      <n-checkbox v-model:checked="formData.remembered" />
    </n-form-item>
    <n-button :disabled="formData.email === '' || formData.password === ''" @click="submitForm">Войти</n-button>
  </n-form>
</template>

<style lang="scss" scoped>

</style>

<script>
const emailStartsWithRegex = /^[a-zA-Z]/;
const emailRegex = /^[\w.-]*@/;
const emailDomainRegex = /(\.[a-zA-Z]{2,})+$/;


const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_(),.+-]).{8,64}$/;

export default {
    name: 'SignInForm',
    data() {
      return {
        formData: {
          email: '',
          password: '',
          remembered: false,
        },
        rules: {
          email: [
            {
              required: true,
              message: 'Пожалуйста, введите email',
              trigger: 'blur'
            },
            {
              validator: (rule, value) => {
                if (!emailStartsWithRegex.test(value)) {
                  return new Error('Email должен начинаться с латинской буквы');
                } else if (!emailRegex.test(value)) {
                  return new Error('Email должен содержать @ и только латинские буквы, цифры, точку, подчеркивание или минус')
                } else if (!emailDomainRegex.test(value)) {
                  return new Error('Домен email должен быть допустимым')
                }
                return true;
              },
              trigger: ['blur', 'input']
            },
          ],
          password: [
            {
              required: true,
              message: 'Пожалуйста, введите пароль',
              trigger: 'blur'
            },
            {
              validator: (rule, value) => passwordRegex.test(value),
              message: 'Пароль должен содержать заглавную букву, строчную букву, цифру и специальный символ',
              trigger: ['blur', 'input']
            }
          ]
        }
      }
    },
    methods: {
      submitForm() {
        // axios.post('', this.formData)
        //   .then((response) => { console.log(response) })
        //   .catch((error) => { console.log(error) });
      }
    }
}
</script>