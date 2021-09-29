<template>
  <div class="sign-in">
    <div class="sign-in-form">
      <div class="input-group-base">
        <label for="si-email">Email</label>
        <input id="si-email" type="email">
      </div>
      <div class="input-group-base">
        <label for="si-password">Пароль</label>
        <input id="si-password" type="password">
      </div>
      <div class="input-group-checkbox">
        <label for="si-checkbox">Запомнить пароль</label>
        <input id="si-checkbox" type="checkbox">
      </div>
      <input type="button" value="Войти">
    </div>
  </div>
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
.sign-in {
  padding-top: 3rem;
  display: flex;
  align-items: center;
  justify-content: center;
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
export default {
  name: 'Sign In',
}
</script>