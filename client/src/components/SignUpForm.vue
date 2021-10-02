<template>
  <n-form>
    <n-form-item label="Email" path="">
      <n-input placeholder=""/>
    </n-form-item>
    <n-form-item label="Пароль" path="">
      <n-input type="password" show-password-on="click" placeholder=""/>
    </n-form-item>
    <n-form-item label="Повторите пароль" path="">
      <n-input type="password" show-password-on="click" placeholder=""/>
    </n-form-item>
    <n-form-item>
      <n-button>Зарегистрироваться</n-button>
    </n-form-item>
  </n-form>
</template>

<style lang="scss" scoped>

</style>

<script>
export default {
    name: 'SignUpForm'
}
</script>