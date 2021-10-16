<template>
	<n-h4><n-text type="primary">Синхронизация с ботом</n-text></n-h4>
	<n-text code>Тут что-то будет</n-text>
	<n-h4><n-text type="primary">Изменение данных</n-text></n-h4>
	<n-space vertical>
		<n-button @click="router.push('/profile/email')">Изменить email</n-button>
		<n-button @click="router.push('/profile/password')">Изменить пароль</n-button>
	</n-space>
	
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useRouter } from "vue-router";

export default defineComponent({
	name: "ProfileSettings",
	setup() {
		const router = useRouter();
		return { router };
	},
});
</script>
