<template>
	<profile-edit-email />
	<profile-edit-password />
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import ProfileEditEmail from '@/components/ProfileEditEmail.vue'
import ProfileEditPassword from '@/components/ProfileEditPassword.vue'

export default defineComponent({
	name: 'ProfileEmail',
	components: {
		ProfileEditEmail, ProfileEditPassword
	}
})
</script>
