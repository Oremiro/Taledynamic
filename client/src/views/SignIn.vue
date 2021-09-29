<template>
  <div class="sign-in">
    <SignInForm></SignInForm>
  </div>
</template>

<style lang="scss" scoped>
.sign-in {
  padding-top: 3rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>

<script>
import SignInForm from '@/components/SignInForm.vue'
export default {
  name: 'Sign In',
  components: {
    SignInForm
  }
}
</script>