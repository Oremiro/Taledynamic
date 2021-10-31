<template>
	<div class="container">
		<n-grid :cols="30" style="max-width: 40rem">
			<n-gi :span="8">
				<n-card
					hoverable
					size="small"
					content-style="padding-left: 0; padding-right: 0"
				>
					<n-menu :options="menuOptions" :value="route.path" />
				</n-card>
			</n-gi>
			<n-gi :span="1" />
			<n-gi :span="21">
				<router-view v-slot="{ Component }">
					<transition name="fade" mode="out-in">
						<n-card hoverable :key="route.path">
							<component :is="Component" />
						</n-card>
					</transition>
				</router-view>
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
import { RouterLink, useRoute, useRouter } from "vue-router";

export default defineComponent({
	name: "Profile",
	setup() {
		const store = useStore();
		const router = useRouter();
		const route = useRoute();
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
				key: '/profile',
			},
						{
				label: () =>
					h(
						RouterLink,
						{ to: '/profile/data' },
						{ default: () => 'Данные' }
					),
				key: '/profile/data',
			},
			{
				label: () =>
					h(
						RouterLink,
						{ to: '/profile/settings' },
						{ default: () => 'Настройки' }
					),
				key: '/profile/settings',
			},
			{
				onClick: async (): Promise<void> => {
					try {
						await store.dispatch('logout')
						router.push({ name: 'Auth'});
						message.info('Вы вышли из аккаунта');
					} catch (error) {
						if(error instanceof Error) {
							message.error(error.message);
						}
					}
				},
				label: 'Выйти',
				key: '/profile/quit',
			}
		];
		return { 
			menuOptions,
			route 
		};
	},
});
</script>
