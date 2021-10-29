<template>
  <div class="container">
    <n-card hoverable style="max-width: 30rem;">
			<n-tabs size="large" v-model:value="activeTab" @update:value="handleUpdateTab">
				<n-tab-pane name="signin" tab="Вход"/>
				<n-tab-pane name="signup" tab="Регистрация"/>
			</n-tabs>
			<router-view v-slot="{ Component }" style="padding-top: 1rem">
				<transition name="fade" mode="out-in">
					<component :is="Component" />
				</transition>
			</router-view>
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
import { onBeforeRouteUpdate, useRouter } from 'vue-router';

export default defineComponent({
  name: 'Auth',
	setup() {
		const router = useRouter();
		const activeTab = ref(router.currentRoute.value.name === 'AuthSignIn' ? 'signin' : 'signup');

		const handleUpdateTab = (value: string) => {
			router.push({ name: value === 'signin' ? 'AuthSignIn' : 'AuthSignUp' })
		}
		onBeforeRouteUpdate(to => {
			activeTab.value = (to.name === 'AuthSignIn' ? 'signin' : 'signup');
		})
		return {
			activeTab,
			handleUpdateTab
		}
	}
})
</script>
