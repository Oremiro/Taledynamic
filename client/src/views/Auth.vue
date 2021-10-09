<template>
  <div class="container">
    <n-card hoverable style="max-width: 30rem">
      <n-tabs default-value="signin" size="large">
        <n-tab-pane name="signin" tab="Вход">
          <sign-in-form />
        </n-tab-pane>
        <n-tab-pane name="signup" tab="Регистрация">
          <sign-up-form />
        </n-tab-pane>
      </n-tabs>
    </n-card>
  </div>
</template>

<style lang="scss" scoped>
.container {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>

<script>
import SignInForm from '@/components/SignInForm.vue'
import SignUpForm from '@/components/SignUpForm.vue'

export default {
  name: 'Sign In',
  components: {
    SignInForm, SignUpForm
  }
}
</script>