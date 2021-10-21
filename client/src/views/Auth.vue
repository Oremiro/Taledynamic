<template>
  <div class="container">
    <n-card hoverable style="max-width: 30rem">
      <n-tabs v-model:value="activeTab" size="large">
        <n-tab-pane name="signin" tab="Вход">
          <sign-in-form />
        </n-tab-pane>
        <n-tab-pane name="signup" tab="Регистрация">
          <sign-up-form @setTab="setTab"/>
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

<script lang="ts">
import { defineComponent, ref } from 'vue'
import SignInForm from '@/components/SignInForm.vue'
import SignUpForm from '@/components/SignUpForm.vue'

export default defineComponent({
  name: 'Sign In',
  components: {
    SignInForm, SignUpForm
  },
	setup() {
		const activeTab = ref<string>('signin');
		const setTab = (name: 'signin' | 'signup') => {
			activeTab.value = name;
		}
		return {
			activeTab,
			setTab
		}
	}
})
</script>
