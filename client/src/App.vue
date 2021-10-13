<template>
	<n-config-provider :theme="currentTheme">
		<n-global-style />
		<n-message-provider>
			<n-layout position="absolute" style="min-height: 100vh">
				<Header @changeTheme="setTheme" :currentTheme="currentTheme" />
				<n-layout-content position="absolute" embedded>
					<router-view />
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
</style>

<script lang="ts">
import { defineComponent, ref } from "vue";
import "vfonts/OpenSans.css";

import { darkTheme, GlobalTheme, useOsTheme } from "naive-ui";
import { useCookie } from "vue-cookie-next";
import Header from "@/components/Header.vue";

type Theme = GlobalTheme | null;

export default defineComponent({
	components: {
		Header,
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
