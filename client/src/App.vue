<template>
	<n-config-provider :theme="currentTheme">
		<n-global-style />
		<n-message-provider>
			<n-layout position="absolute" style="min-height: 100vh">
				<n-loading-bar-provider>
					<layout-header @changeTheme="setTheme" :currentTheme="currentTheme" />
				</n-loading-bar-provider>
				<n-layout-content position="absolute" embedded>
					<router-view v-slot="{ Component }">
						<transition name="fade" mode="out-in">
							<component :is="Component" />
						</transition>
					</router-view>
				</n-layout-content>
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
</style>

<script lang="ts">
import "vfonts/OpenSans.css";
import { defineComponent, ref } from "vue";
import { darkTheme, useOsTheme } from "naive-ui";
import { useCookie } from "vue-cookie-next";
import LayoutHeader from "@/layouts/Header.vue";
import { Theme } from "@/interfaces";



export default defineComponent({
	components: {
		LayoutHeader
	},
	setup() {
		const currentTheme = ref<Theme>(darkTheme);
		const cookie = useCookie();
		const cookieTheme: string | null = cookie.getCookie("theme");
		if (cookieTheme === null) {
			const osThemeRef = useOsTheme();
			currentTheme.value = osThemeRef.value === "dark" ? darkTheme : null;
		} else {
			currentTheme.value = cookieTheme === "dark" ? darkTheme : null;
		}
		const setTheme = (value: Theme) => {
			currentTheme.value = value;
			cookie.setCookie("theme", value === null ? "light" : "dark", {
				expire: Infinity,
			});
		};
		return {
			currentTheme,
			setTheme,
		};
	},
});
</script>
