<template>
	<n-h4><n-text type="primary">Изменение email</n-text></n-h4>
	<n-form>
		<n-form-item path="email.value">
			<n-auto-complete @update:value="submitButtonShown = true" placeholder="" default-value="1asagne@mail.ru" />
		</n-form-item>
		<n-button v-if="submitButtonShown">Сохранить изменения</n-button>
	</n-form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'

export default defineComponent({
	name: 'ProfileEmail',
	setup() {
		const submitButtonShown = ref<boolean>(false);
		return {
			submitButtonShown
		}
	}
})
</script>
