<template>
  <form novalidate class="sign-in-form" @submit.prevent="submitForm">
    <div class="input-group-base">
      <label for="si-email">Email</label>
      <input id="si-email" type="email" v-model="formData.email">
      <div v-if="v$.formData.email.$error">Email field has an error.</div>
    </div>
    <div class="input-group-base">
      <label for="si-password">Пароль</label>
      <input id="si-password" type="password" v-model="formData.password">
    </div>
    <div class="input-group-checkbox">
      <label for="si-checkbox">Запомнить пароль</label>
      <input id="si-checkbox" type="checkbox" v-model="formData.remembered">
    </div>
    <button type="submit">Войти</button>
  </form>
</template>

<style lang="scss" scoped>
input {
  padding: .6rem .7rem;
  border: .05rem solid #ddd;
  border-radius: .2rem;
}
input[type=button] {
  cursor: pointer;
}
%input-group {
  display: flex;
  align-items: flex-start;
}
%input-group:not(:last-child) {
  margin-bottom: 1rem;
}
.input-group-base {
  @extend %input-group;
  flex-direction: column;
  label {
    margin-bottom: .25rem;
  }
}
.input-group-checkbox {
  @extend %input-group;
  flex-direction: row;
  label {
    margin-right: .5rem;
  }
}
.sign-in-form {
  max-width: 30rem;
  display: flex;
  flex-direction: column;
  padding: 1rem 1.5rem;
  border: .05rem solid #ddd;
  border-radius: .2rem;
}
</style>

<script>
import axios from 'axios'
import useVuelidate from '@vuelidate/core'
import { required, email } from '@vuelidate/validators'

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
          email: { required, email },
          password: { required },
        }
      }
    },
    methods: {
      validateForm() {
        console.log('Validation');
      },
      submitForm() {
        axios.post('', this.formData)
          .then((response) => { console.log(response) })
          .catch((error) => { console.log(error) });
      }
    }
}
</script>