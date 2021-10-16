<template>
	<n-h4><n-text type="primary">Изменение пароля</n-text></n-h4>
	<n-form>
		<n-form-item label="Пароль" path="password.value">
			<n-input
				type="password"
				show-password-on="click"
				placeholder=""
				:disabled="currentPasswordDisabled"
			/>
		</n-form-item>
		<n-form-item label="Новый пароль" path="password.value">
			<n-input type="password" show-password-on="click" placeholder="" />
		</n-form-item>
		<n-form-item label="Повторите пароль" path="password.value">
			<n-input type="password" show-password-on="click" placeholder="" />
		</n-form-item>
		<n-button>Изменить пароль</n-button>
	</n-form>
</template>

<script lang="ts">
import { defineComponent } from 'vue'

export default defineComponent({
	name: 'ProfilePassword'
})
</script>
