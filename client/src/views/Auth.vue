<template>
  <div class="container">
    <n-card hoverable style="max-width: 30rem">
      <n-tabs :value="activeTab" @update:value="handleUpdateTab" size="large">
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

<script lang="ts">
import { defineComponent, computed } from 'vue'
import SignInForm from '@/components/SignInForm.vue'
import SignUpForm from '@/components/SignUpForm.vue'
import { useRoute, useRouter } from 'vue-router'

export default defineComponent({
  name: 'Sign In',
	components: {
		SignInForm, SignUpForm
	},
	setup() {
		const router = useRouter();
		const route = useRoute();
		
		const activeTab = computed(() => (route.name === 'AuthSignIn' ? 'signin' : 'signup'));

		const handleUpdateTab = (value: string | number) => {
			router.push({ name: value === 'signin' ? 'AuthSignIn' : 'AuthSignUp' })
		}
		return {
			activeTab,
			handleUpdateTab
		}
	}
})
</script>
