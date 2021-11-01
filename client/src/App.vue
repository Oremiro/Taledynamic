<template>
	<n-config-provider :theme="currentTheme">
		<n-global-style />
		<n-message-provider>
			<n-layout style="min-height: 100vh">
				<n-loading-bar-provider>
				<layout-header @changeTheme="setTheme" :currentTheme="currentTheme" />
				</n-loading-bar-provider>
				<n-layout has-sider position="absolute" style="top: 3.3rem;">
					<n-layout-sider 
						:native-scrollbar="false" 
						bordered
						collapse-mode="transform"
						show-trigger="bar"
						:collapsed-width="0"
						width="16rem"
						:default-collapsed="true"
						content-style="padding: 1rem 0">
						<workspaces-list />
					</n-layout-sider>
					<n-layout-content embedded style="min-width: 100%; min-height: 100%;">
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
  transition: all .3s ease-out;
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
import { useCookie } from "vue-cookie-next";
import LayoutHeader from "@/layouts/Header.vue";
import WorkspacesList from "./components/workspaces/WorkspacesList.vue";
import { Theme } from "@/interfaces";


// data
const currentTheme = ref<Theme>(darkTheme);
const cookie = useCookie();
const cookieTheme: string | null = cookie.getCookie("theme");

if (cookieTheme === null) {
	const osThemeRef = useOsTheme();
	currentTheme.value = osThemeRef.value === "dark" ? darkTheme : null;
} else {
	currentTheme.value = cookieTheme === "dark" ? darkTheme : null;
}

// methods
const setTheme = (value: Theme) => {
	currentTheme.value = value;
	cookie.setCookie("theme", value === null ? "light" : "dark", {
		expire: Infinity,
	});
};
</script>
