<template>
	<n-config-provider :theme="currentTheme">
		<n-global-style />
		<n-message-provider>
			<n-layout style="min-height: 100vh">
				<n-loading-bar-provider>
					<layout-header @changeTheme="setTheme" :currentTheme="currentTheme" />
				</n-loading-bar-provider>
				<n-layout has-sider position="absolute" style="top: 3.3rem;">
					<layout-sider />
					<n-layout-content embedded>
						<router-view v-slot="{ Component }">
							<transition name="fade" mode="out-in">
								<component :is="Component" />
							</transition>
						</router-view>
					</n-layout-content>
				</n-layout>
			</n-layout>
		</n-message-provider>
	</n-config-provider>
</template>

<style lang="scss">
#app {
	font-family: v-sans;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity .3s ease-out;
}

.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

.container {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>

<script setup lang="ts">
import "vfonts/OpenSans.css";
import { ref } from "@vue/reactivity";
import { darkTheme, useOsTheme } from "naive-ui";
import LayoutHeader from "@/layouts/LayoutHeader.vue";
import LayoutSider from "@/layouts/LayoutSider.vue";
import { Theme } from "@/interfaces";
import { useRouter } from "vue-router";
import { useStore } from "@/store";


const currentTheme = ref<Theme>(darkTheme);
const storedTheme: string | null = localStorage.getItem('theme');

if (storedTheme) {
	currentTheme.value = storedTheme === 'dark' ? darkTheme : null;
} else {
	const osThemeRef = useOsTheme();
	currentTheme.value = osThemeRef.value === 'dark' ? darkTheme : null;
}

function setTheme(value: Theme): void {
	currentTheme.value = value;
	localStorage.setItem('theme', value === null ? 'light' : 'dark');
}

const router = useRouter();
const store = useStore();

onstorage = event => {
	if (event.key === 'theme') {
		currentTheme.value = event.newValue === 'dark' ? darkTheme : null;
	} else if (event.key === 'user' && event.newValue === null) {
		store.commit('user/logout');
		router.push({ name: 'AuthSignIn'});
	}
}
</script>
