<template>
	<div class="container">
		<n-grid :cols="30" style="max-width: 40rem">
			<n-gi :span="8">
				<n-card
					hoverable
					style="min-height: 100%"
					size="small"
					content-style="padding-left: 0; padding-right: 0"
				>
					<n-menu :options="menuOptions" :value="$route.path" />
				</n-card>
			</n-gi>
			<n-gi :span="1" />
			<n-gi :span="21">
				<n-card hoverable>
					<router-view />
				</n-card>
			</n-gi>
		</n-grid>
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
import { useStore } from "@/store";
import { MenuOption, useMessage } from "naive-ui";
import { defineComponent, h } from "vue";
import { RouterLink } from "vue-router";

export default defineComponent({
	name: "Profile",
	setup() {
		const store = useStore();
		const message = useMessage();
		const menuOptions: MenuOption[] = [
			{
				label: () =>
					h(
						RouterLink,
						{
							to: "/profile",
						},
						{ default: () => "Профиль" }
					),
				key: "/profile",
			},
			{
				label: () =>
					h(
						RouterLink,
						{
							to: "/profile/settings",
						},
						{ default: () => "Настройки" }
					),
				key: "/profile/settings",
			},
			{
				onClick: () => {
					store.dispatch('logout')
					.then((response) => {
						message.success(response);
					})
					.catch((error) => {
						message.error(error.response);
					})
				},
				label: 'Выйти',
				key: '/profile/quit',
			}
		];
		return { menuOptions };
	},
});
</script>
